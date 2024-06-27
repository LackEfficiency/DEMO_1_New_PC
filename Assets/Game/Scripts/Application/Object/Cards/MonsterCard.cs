using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class MonsterCard : Card
{
    #region ����
    public const float CLOSED_DISTANCE = 0.1f; //���ڸþ���Ĭ�ϵ���
    public const float MOVE_SPEED = 2f;
    #endregion

    #region �¼�
    public event Action<MonsterCard> StatusChanged; //״̬����
    public event Action<Card> Dead; //����

    //������Ϊ ����BUFF����
    public event Action<MonsterCard> OnMove;
    public event Action<MonsterCard> OnAttack;
    public event Action<MonsterCard> OnDamage;
    public event Action<MonsterCard> OnDie;
    public event Action<MonsterCard> OnTakeDamage;
    public event Action<MonsterCard> OnTakeHeal;
    public event Action<MonsterCard> OnActionFinish;
    public event Action<MonsterCard> OnActionStart;
    #endregion

    #region �ֶ�
    private Vector3 curPos; //���浱ǰλ��
    private Vector3 nextDes; //�����¸��ƶ�λ��
    private MonsterCard target; //���湥��Ŀ�� 

    bool m_IsCardReturn = false; //�ж��Ƿ����ڷ���
    bool m_IsCardMoving = false; //�ж��Ƿ���Ҫ�ƶ�
    bool m_IsCardAttacking = false; //�ж��Ƿ���Ҫ����  

    private MonsterCardInfo monsterCardInfo; //������Ϣ

    //��������
    private int m_BaseAttack; //����������
    private int m_Hp; //����Ѫ��
    private int m_MaxHp; //���Ѫ��
    private int m_MoveRange; //�ƶ���Χ
    private int m_AttackRange; //������Χ

    private int m_AttackBoost; //����������
    private int m_MaxHpBoost; //�������ֵ����

    #endregion

    #region ����
    //����Ѫ���Ƿ����仯
    public int Hp
    {
        get => m_Hp;
        set
        {
            //��ΧԼ��
            value = Mathf.Clamp(value, 0, MaxHp + MaxHpBoost);

            //�����ظ����������
            if (value == m_Hp)
                return;

            //�����¼�
            if (value < m_Hp)
            {
                if (OnTakeDamage != null)
                {
                    OnTakeDamage(this);
                }
            }

            //�����¼�
            if (value > m_Hp)
            {
                if (OnTakeHeal != null)
                {
                    OnTakeHeal(this);
                }
            }

            //��ֵ
            m_Hp = value;

            //Ѫ���仯�¼� 
            if (StatusChanged != null)
                StatusChanged(this);


            //�����¼�
            if (m_Hp == 0)
            {
                if (Dead != null)
                {
                    Dead(this);
                }
            }

        }
    }

    //���ƹ�����
    public int TotalAttack
    {
        get => Math.Max(0, BaseAttack + AttackBoost);
    }

    //�����Ƿ�����
    public bool IsDead
    {
        get { return m_Hp == 0; }
    }

    //�����Ƿ������ƶ�
    public bool IsCardMoving
    {
        get => m_IsCardMoving;
        set => m_IsCardMoving = value;
    }

    //������һ���ƶ�λ�� 
    public Vector3 NextDes
    {
        get => nextDes;
        set => nextDes = value;
    }

    //�����Ƿ����ڹ���
    public bool IsCardAttacking
    {
        get => m_IsCardAttacking;
        set => m_IsCardAttacking = value;
    }

    //���ƵĹ�������
    public MonsterCard Target
    {
        get => target;
        set => target = value;
    }

    //���Ƶ�ǰλ��
    public Vector3 CurPos
    {
        get => curPos;
        set => curPos = value;
    }

    //�Ƿ񹥻�������ڷ���
    public bool IsCardReturn
    {
        get => m_IsCardReturn;
        set => m_IsCardReturn = value;
    }

    //������Ϣ
    public MonsterCardInfo MonsterCardInfo
    {
        get => monsterCardInfo;
        set => monsterCardInfo = value;
    }

    public int AttackBoost { get => AttackBoost1; set => AttackBoost1 = value; }
    public int BaseAttack { get => m_BaseAttack; set => m_BaseAttack = value; }
    public int MaxHp { get => m_MaxHp; set => m_MaxHp = value; }
    public int MoveRange { get => m_MoveRange; set => m_MoveRange = value; }
    public int AttackRange { get => m_AttackRange; set => m_AttackRange = value; }
    public int AttackBoost1 { get => m_AttackBoost; set => m_AttackBoost = value; }
    public int MaxHpBoost { get => m_MaxHpBoost; set => m_MaxHpBoost = value; }



    #endregion

    #region ����
    //ˢ������
    public void UpdateStats()
    {
        this.BaseAttack = MonsterCardInfo.Attack;
        this.MaxHp = MonsterCardInfo.MaxHP;
        this.m_Hp = MonsterCardInfo.HP;
        this.MoveRange = MonsterCardInfo.MoveRange;
        this.AttackRange = MonsterCardInfo.AttackRange;
    }

    //�ƶ���Ŀ��λ��
    void MoveTo(Vector3 dest)
    {
        transform.position = dest;
    }

    //�ƶ��߼�, ƽ���ƶ�
    public void Move()
    {
        //��ȡ��ǰλ��
        Vector3 pos = transform.position;

        //�������
        float dis = Vector3.Distance(pos, NextDes);

        if (dis < CLOSED_DISTANCE)
        {
            MoveTo(NextDes);
            IsCardMoving = false;

            //�ƶ�����¼�
            if (OnMove != null)
            {
                OnMove(this);
            }
        }
        else
        {
            //�����ƶ�����
            Vector3 direction = (NextDes - pos).normalized;

            //ƽ���ƶ�(��/֡ = ��/�� * Time.deltaTime)
            transform.Translate(direction * MOVE_SPEED * Time.deltaTime);
        }
    }

    //�����߼�����������
    //TODO: ��������
    public void Attack() //���빥��Ŀ���λ�ú͵�ǰ��λ��
    {
        //������ʱ�����ƶ��������湥������
        //�ƶ���Ŀ��λ�ú󷵻ص�ǰλ��

        //��ȡ��ǰλ��
        Vector3 pos = transform.position;

        //�������
        float dis = Vector3.Distance(pos, NextDes);

        if (dis < CLOSED_DISTANCE)
        {
            MoveTo(NextDes);
            NextDes = CurPos;
            //��һ�εִ���Ŀ��λ����Ϊ��ʼλ�ã�ͬʱ�����ڷ�������Ϊtrue
            //�ڶ��εִ������ڷ�������Ϊfalse��ͬʱ���ڹ�������Ϊfalse
            if (IsCardReturn == true)
            {
                IsCardAttacking = false;
                IsCardReturn = false;
                //�������Ŀ��Ƽ�Ѫ
                target.Damage(TotalAttack);

                //�����¼�
                if (OnAttack != null)
                {
                    OnAttack(this);
                }

                //��������˺��¼�
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
            //�����ƶ�����
            Vector3 direction = (NextDes - pos).normalized;

            //ƽ���ƶ�(��/֡ = ��/�� * Time.deltaTime)
            transform.Translate(direction * MOVE_SPEED * Time.deltaTime);
        }


    }

    //����
    public void Damage(int hit)
    {
        if (IsDead)
            return;

        Hp -= hit;

        //ToDO:��������������
  
        Debug.Log(monsterCardInfo.CardName.ToString() + ":��������");
    }

    //�����¼�����
    protected void Die(Card card)
    {
        Debug.Log(monsterCardInfo.CardName.ToString() + ":Die");
    }

    //����������
    public void IncreaseDamage(int value)
    {
        if (value == 0) return;

        this.AttackBoost += value;
        if (StatusChanged != null)
            StatusChanged(this);
    }

    //�������ֵ����
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
    #endregion

    #region Unity�ص�
    //��������
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

    #region �¼��ص�
    public override void OnSpawn()
    {
        base.OnSpawn();
        this.Dead += Die;
        this.StatusChanged += StatusChanged;

        //��ʼ�����⹥����
        this.AttackBoost = 0;
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

    #region ��������
    #endregion
}
