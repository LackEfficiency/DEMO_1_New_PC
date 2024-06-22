using System;
using System.Collections;
using System.Collections.Generic;
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
    #endregion

    #region 字段
    private Vector3 curPos; //保存当前位置
    private Vector3 nextDes; //保存下个移动位置
    private MonsterCard target; //保存攻击目标 

    bool m_IsCardReturn = false; //判断是否正在返回
    bool m_IsCardMoving = false; //判断是否需要移动
    bool m_IsCardAttacking = false; //判断是否需要攻击  

    private MonsterCardInfo monsterCardInfo; //卡牌信息

    #endregion

    #region 属性
    //卡牌血量是否发生变化
    public int Hp
    {
        get => monsterCardInfo.HP;
        set
        {
            //范围约定
            value = Mathf.Clamp(value, 0, monsterCardInfo.MaxHP);

            //减少重复，提高性能
            if (value == monsterCardInfo.HP)
                return;

            //赋值
            monsterCardInfo.HP = value;

            //血量变化事件 
            if (StatusChanged != null)
                StatusChanged(this);

            //死亡事件
            if (monsterCardInfo.HP == 0)
            {
                if (Dead != null)
                {
                    Dead(this);
                }
            }

        }
    }

    //卡牌是否死亡
    public bool IsDead
    {
        get { return monsterCardInfo.HP == 0; }
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


    #endregion

    #region 方法
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
                target.Damage(monsterCardInfo.Attack);

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

    //受伤
    public void Damage(int hit)
    {
        if (IsDead)
            return;

        Hp -= hit;

        //ToDO:触发动画触发器
  
        Debug.Log(monsterCardInfo.CardName.ToString() + ":卡牌受伤");
    }

    //死亡事件触发
    protected void Die(Card card)
    {
        Debug.Log(monsterCardInfo.CardName.ToString() + ":Die");
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
        this.MonsterCardInfo = this.GetComponent<UIUnitStatus>().CardInfo as MonsterCardInfo;
        this.Dead += Die;
        this.StatusChanged += StatusChanged;
    }

    public override void OnUnspawn()
    {
        base.OnUnspawn();

        while (StatusChanged != null)
        {
            StatusChanged -= StatusChanged;
        }

        while (Dead != null)
        {
            Dead -= Dead;
        }
    }
    #endregion

    #region 帮助方法
    #endregion
}
