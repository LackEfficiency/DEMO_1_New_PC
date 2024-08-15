using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class MonsterCard : Card
{
    #region 常量
    public const float CLOSED_DISTANCE = 0.1f; //低于该距离默认到达
    public const float MOVE_SPEED = 2f;
    #endregion

    #region 事件
    public event Action<MonsterCard> StatusChanged; //状态更新
    public event Action<Card> Dead; //死亡

    //各类行为 用于BUFF更新
    public event Action<MonsterCard> OnMove;
    public event Action<MonsterCard, MonsterCard> OnAttack;
    public event Action<MonsterCard> OnDamage;
    public event Action<MonsterCard> OnDie;
    public event Action<MonsterCard, MonsterCard> OnTakeDamage;
    public event Action<MonsterCard> OnTakeHeal;
    public event Action<MonsterCard> OnActionFinish;
    public event Action<MonsterCard> OnActionStart;
    #endregion

    #region 字段
    private Vector3 curPos; //保存当前位置
    private Vector3 nextDes; //保存下个移动位置
    private MonsterCard target; //保存攻击目标 

    bool m_IsCardReturn = false; //判断是否正在返回
    bool m_IsCardMoving = false; //判断是否需要移动
    bool m_IsCardAttacking = false; //判断是否需要攻击  

    private MonsterCardInfo monsterCardInfo; //卡牌信息

    private MonsterCard attacker; //攻击者

    //基本属性
    private int m_BaseAttack; //基础攻击力
    protected int m_Hp; //基础血量
    private int m_MaxHp; //最大血量
    private int m_MoveRange; //移动范围
    private int m_AttackRange; //攻击范围

    private int m_AttackBoost; //攻击力增加
    private int m_MaxHpBoost; //最大生命值增加

    //技能用属性
    private int m_IsGuardian; //是否触发守护状态 优先进行攻击 范围内不存在可攻击对象时 再进行移动
                              //大于0则存在守护状态
    private int m_CantAction; //是否可以行动 大于0则不可以行动

    

    #endregion

    #region 属性
    //卡牌血量是否发生变化
    public int Hp
    {
        get => m_Hp;
        set
        {
            //范围约定
            value = Mathf.Clamp(value, 0, MaxHp + MaxHpBoost);

            //减少重复，提高性能
            if (value == m_Hp)
                return;

            //受伤事件
            if (value < m_Hp)
            {
                if (OnTakeDamage != null)
                {
                    OnTakeDamage(attacker, this);
                }
            }

            //治疗事件
            if (value > m_Hp)
            {
                if (OnTakeHeal != null)
                {
                    OnTakeHeal(this);
                }
            }

            //赋值
            m_Hp = value;

            //血量变化事件 
            if (StatusChanged != null)
                StatusChanged(this);


            //死亡事件
            if (m_Hp == 0)
            {
                if (Dead != null)
                {
                    Dead(this);
                }
            }

        }
    }

    //卡牌攻击力
    public int TotalAttack
    {
        get => Math.Max(0, BaseAttack + AttackBoost);

    }

    //卡牌是否死亡
    public bool IsDead
    {
        get { return m_Hp == 0; }
    }

    //卡牌是否正在移动
    public bool IsCardMoving
    {
        get => m_IsCardMoving;
        set => m_IsCardMoving = value;
    }

    //卡牌下一个移动位置 
    public Vector3 NextDes
    {
        get => nextDes;
        set => nextDes = value;
    }

    //卡牌是否正在攻击
    public bool IsCardAttacking
    {
        get => m_IsCardAttacking;
        set => m_IsCardAttacking = value;
    }

    //卡牌的攻击对象
    public MonsterCard Target
    {
        get => target;
        set => target = value;
    }

    //卡牌当前位置
    public Vector3 CurPos
    {
        get => curPos;
        set => curPos = value;
    }

    //是否攻击完毕正在返回
    public bool IsCardReturn
    {
        get => m_IsCardReturn;
        set => m_IsCardReturn = value;
    }

    //卡牌信息
    public MonsterCardInfo MonsterCardInfo
    {
        get => monsterCardInfo;
        set => monsterCardInfo = value;
    }

    public int BaseAttack { get => m_BaseAttack; set => m_BaseAttack = value; }
    public int MaxHp { get => m_MaxHp; set => m_MaxHp = value; }
    public int MoveRange { get => m_MoveRange; set => m_MoveRange = value; }
    public int AttackRange { get => m_AttackRange; set => m_AttackRange = value; }
    public int AttackBoost { get => m_AttackBoost; set => m_AttackBoost = value; }
    public int MaxHpBoost { get => m_MaxHpBoost; set => m_MaxHpBoost = value; }
    public int IsGuardian { get => m_IsGuardian; set => m_IsGuardian = value; }
    public int CantAction { get => m_CantAction; set => m_CantAction = value; }



    #endregion

    #region 方法
    //刷新属性
    public void InitStatus()
    {
        this.BaseAttack = MonsterCardInfo.Attack;
        this.MaxHp = MonsterCardInfo.MaxHP;
        this.m_Hp = MonsterCardInfo.HP;
        this.MoveRange = MonsterCardInfo.MoveRange;
        this.AttackRange = MonsterCardInfo.AttackRange;
    }

    //移动到目标位置
    void MoveTo(Vector3 dest)
    {
        transform.position = dest;
    }

    //移动逻辑, 平滑移动
    public void Move()
    {
        //获取当前位置
        Vector3 pos = transform.position;

        //计算距离
        float dis = Vector3.Distance(pos, NextDes);

        if (dis < CLOSED_DISTANCE)
        {
            MoveTo(NextDes);
            IsCardMoving = false;

            //移动完毕事件
            if (OnMove != null)
            {
                OnMove(this);
            }
        }
        else
        {
            //计算移动方向
            Vector3 direction = (NextDes - pos).normalized;

            //平滑移动(米/帧 = 米/秒 * Time.deltaTime)
            transform.Translate(direction * MOVE_SPEED * Time.deltaTime);
        }
    }

    //攻击逻辑，攻击动画
    //TODO: 攻击动画
    public void Attack() //传入攻击目标的位置和当前的位置
    {
        //这里暂时先用移动动作代替攻击动作
        //移动到目标位置后返回当前位置

        //获取当前位置
        Vector3 pos = transform.position;

        //计算距离
        float dis = Vector3.Distance(pos, NextDes);

        if (dis < CLOSED_DISTANCE)
        {
            MoveTo(NextDes);
            NextDes = CurPos;
            //第一次抵达后把目标位置设为初始位置，同时把正在返回设置为true
            //第二次抵达后把正在返回设置为false，同时正在攻击设置为false
            if (IsCardReturn == true)
            {
                IsCardAttacking = false;
                IsCardReturn = false;
                //被攻击的卡牌减血
                target.Damage(this, TotalAttack);

                //攻击事件
                if (OnAttack != null)
                {
                    OnAttack(this, target);
                }

                //攻击造成伤害事件
                if (OnDamage != null)
                {
                    OnDamage(this);
                }

            }
            else
                IsCardReturn = true;
        }
        else
        {
            //计算移动方向
            Vector3 direction = (NextDes - pos).normalized;

            //平滑移动(米/帧 = 米/秒 * Time.deltaTime)
            transform.Translate(direction * MOVE_SPEED * Time.deltaTime);
        }
    }

    //重载 传入攻击者和目标 触发攻击动作
    public void Attack(MonsterCard attacker, MonsterCard target)
    {
        CardAction cardAction = GameObject.Find("Map").GetComponent<CardAction>();
        StartCoroutine(cardAction.CardAttack(attacker, target));
    }


    //受伤
    public void Damage(MonsterCard attacker = null, int hit = 0)
    {
        if (IsDead)
            return;

        //更新攻击者
        this.attacker = attacker;

        Hp -= hit;

        //ToDO:触发动画触发器
    }

    //外部调用的死亡
    public void Kill()
    {
        Hp = 0;
    }

    //死亡事件触发
    protected void Die(Card card)
    {
        //销毁卡牌
        Game.Instance.ObjectPool.Unspawn(this.gameObject);
    }

    //攻击力增加
    public void IncreaseDamage(int value)
    {
        if (value == 0) return;

        this.AttackBoost += value;
        if (StatusChanged != null)
            StatusChanged(this);
    }

    //最大生命值增加
    public void IncreaseMaxHp(int value)
    {
        this.MaxHpBoost += value;
        this.Hp += value;
    }

    public void ActionFinish()
    {
        if (OnActionFinish != null)
        {
            OnActionFinish(this);
        }
    }

    public void OnStatusChanged(MonsterCard monsterCard)
    {
        if (StatusChanged != null)
        {
            StatusChanged(this);
        }
    }
    #endregion

    #region Unity回调
    //动画控制
    private void Update()
    {
        if (IsCardMoving)
        {
            Move();
        }

        if (IsCardAttacking)
        {
            Attack();
        }
    }

    #endregion

    #region 事件回调
    public override void OnSpawn()
    {
        base.OnSpawn();
        this.Dead += Die;
        //初始化额外攻击力
        this.AttackBoost = 0;
        //重置Buff
        Game.Instance.BuffManager.RemoveAllBuff(this);

        //重置技能
        Game.Instance.SkillManager.RemoveAllSkills(this);
    }

    public override void OnUnspawn()
    {
        base.OnUnspawn();

        //移除事件
        while (StatusChanged != null)
        {
            StatusChanged -= StatusChanged;
        }

        while (Dead != null)
        {
            Dead -= Dead;
        }

        this.BaseAttack = 0;
        this.MaxHp = 0;
        this.Hp = 0;
        this.MoveRange = 0;
        this.AttackRange = 0;


        this.AttackBoost = 0;

        while (OnMove != null)
        {
            OnMove -= OnMove;
        }   
        while (OnAttack != null)
        {
            OnAttack -= OnAttack;
        }   
        while (OnDamage != null)
        {
            OnDamage -= OnDamage;
        }
        while (OnDie != null)
        {
            OnDie -= OnDie;
        }
        while (OnTakeDamage != null)
        {
            OnTakeDamage -= OnTakeDamage;
        }
        while (OnTakeHeal != null)
        {
            OnTakeHeal -= OnTakeHeal;
        }
        while (OnActionFinish != null)
        {
            OnActionFinish -= OnActionFinish;
        }
        while (OnActionStart != null)
        {
            OnActionStart -= OnActionStart;
        }
        
    }
    #endregion

    #region 帮助方法
    #endregion
}
