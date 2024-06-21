using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using static UnityEditor.PlayerSettings;

public class Card: Reusable, IReusable
{
    #region ����
    public const float CLOSED_DISTANCE = 0.1f; //���ڸþ���Ĭ�ϵ���
    public const float MOVE_SPEED = 2f;
    #endregion

    #region �¼�
    public event Action<Card> CardPosistionChange; //����λ���ƶ�ʱ(�������뿪) ���ն���
    #endregion

    #region �ֶ�
    public Player player = Player.Self; //�ж��ٻ������ѷ����ǵз�
    bool m_PosState; //��⿨��λ���Ƿ�仯(�������ƶ������ϣ������ƶ���Ĺ�أ�����Ϊtrue�������仯
    private int m_Counter = 0; //��deckmanager�����м�⿨������

    private Vector3 curPos; //���浱ǰλ��
    private Vector3 nextDes; //�����¸��ƶ�λ��
    private Card target; //���湥��Ŀ�� 

    bool m_IsCardReturn = false; //�ж��Ƿ����ڷ���
    bool m_IsCardMoving = false; //�ж��Ƿ���Ҫ�ƶ�
    bool m_IsCardAttacking = false; //�ж��Ƿ���Ҫ����  

  

    #endregion

    #region ����
    //����λ���Ƿ����仯
    public bool PosState
    {
        get { return m_PosState; }
        set
        {
            m_PosState = value;
            if (m_PosState == true)
            {
                CardPosistionChange?.Invoke(this);
            }
        }
    }

    public int Counter
    {
        get => m_Counter;
        set
        {
            m_Counter += value;
            if (m_Counter == 0)
            {
                CardPosistionChange?.Invoke(this);
            }
        }
    }

    public bool IsCardMoving 
    { 
        get => m_IsCardMoving; 
        set => m_IsCardMoving = value; 
    }


    public Vector3 NextDes 
    {
        get => nextDes; 
        set => nextDes = value; 
    }

    public Player Player 
    { 
        get => player; 
        set => player = value; 
    }

    public bool IsCardAttacking 
    { 
        get => m_IsCardAttacking; 
        set => m_IsCardAttacking = value; 
    }

    public Card Target 
    { 
        get => target; 
        set => target = value; 
    }
    public Vector3 CurPos 
    { 
        get => curPos; 
        set => curPos = value; 
    }
    public bool IsCardReturn 
    { 
        get => m_IsCardReturn; 
        set => m_IsCardReturn = value; 
    }
    #endregion

    #region ����
    public void PositionChange(Card card)
    {

    }

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
            }
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
        this.CardPosistionChange += PositionChange;
        this.PosState = false;
    }

    public override void OnUnspawn()
    {
        //��ԭ������
        m_Counter = 0;
        m_PosState = false;
        while (CardPosistionChange != null)
        {
            CardPosistionChange -= CardPosistionChange;
        }
    }
    #endregion

    #region ��������
    #endregion
}
