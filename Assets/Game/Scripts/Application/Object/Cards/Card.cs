using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
    private Vector3 nextDes; //�����¸��ƶ�λ��
    bool m_IsCardMoving; //�ж��Ƿ���Ҫ�ƶ�
  

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
    #endregion

    #region ����
    public void PositionCHange(Card card)
    {

    }

    void MoveTo(Vector3 dest)
    {
        transform.position = dest;
    }

    public void Move(Vector3 dest)
    {
        //��ȡ��ǰλ��
        Vector3 pos = transform.position;

        //�������
        float dis = Vector3.Distance(pos, dest);

        if (dis < CLOSED_DISTANCE)
        {
            MoveTo(dest);
            IsCardMoving = false;
        }
        else
        {
            //�����ƶ�����
            Vector3 direction = (dest - pos).normalized;

            //ƽ���ƶ�(��/֡ = ��/�� * Time.deltaTime)
            transform.Translate(direction * MOVE_SPEED * Time.deltaTime);
        }
    }
    #endregion

    #region Unity�ص�
    private void Update()
    {
        if (IsCardMoving)
        {
            Move(NextDes);
        }
    }
    #endregion

    #region �¼��ص�
    public override void OnSpawn()
    {
        this.CardPosistionChange += PositionCHange;
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
