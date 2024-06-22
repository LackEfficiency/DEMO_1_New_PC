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
    #endregion

    #region �ֶ�
    private Vector3 curPos; //���浱ǰλ��
    private Vector3 nextDes; //�����¸��ƶ�λ��
    private MonsterCard target; //���湥��Ŀ�� 

    bool m_IsCardReturn = false; //�ж��Ƿ����ڷ���
    bool m_IsCardMoving = false; //�ж��Ƿ���Ҫ�ƶ�
    bool m_IsCardAttacking = false; //�ж��Ƿ���Ҫ����  

    private MonsterCardInfo monsterCardInfo; //������Ϣ

    #endregion

    #region ����
    //����Ѫ���Ƿ����仯
    public int Hp
    {
        get => monsterCardInfo.HP;
        set
        {
            //��ΧԼ��
            value = Mathf.Clamp(value, 0, monsterCardInfo.MaxHP);

            //�����ظ����������
            if (value == monsterCardInfo.HP)
                return;

            //��ֵ
            monsterCardInfo.HP = value;

            //Ѫ���仯�¼� 
            if (StatusChanged != null)
                StatusChanged(this);

            //�����¼�
            if (monsterCardInfo.HP == 0)
            {
                if (Dead != null)
                {
                    Dead(this);
                }
            }

        }
    }

    //�����Ƿ�����
    public bool IsDead
    {
        get { return monsterCardInfo.HP == 0; }
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


    #endregion

    #region ����
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
                target.Damage(monsterCardInfo.Attack);

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

    #region ��������
    #endregion
}
