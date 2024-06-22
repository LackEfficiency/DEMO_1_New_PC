using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using static UnityEditor.PlayerSettings;

public class Card: Reusable, IReusable
{
    #region ����
    #endregion

    #region �¼�
    public event Action<Card> CardPosistionChange; //����λ���ƶ�ʱ(�������뿪) ���ն���
    #endregion

    #region �ֶ�
    public Player player = Player.Self; //�ж��ٻ������ѷ����ǵз�
    bool m_PosState; //��⿨��λ���Ƿ�仯(�������ƶ������ϣ������ƶ���Ĺ�أ�����Ϊtrue�������仯
    private int m_Counter = 0; //��deckmanager�����м�⿨������
    private CardInfo cardInfo; //������Ϣ

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

    //����������¼
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

    //�ٿؿ��Ƶ����
    public Player Player 
    { 
        get => player; 
        set => player = value; 
    }

    //������Ϣ 
    public CardInfo CardInfo
    {
        get => cardInfo;
        set => cardInfo = value;
    }
    #endregion

    #region ����
    public void PositionChange(Card card)
    { 

    }
    #endregion

    #region Unity�ص�
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
