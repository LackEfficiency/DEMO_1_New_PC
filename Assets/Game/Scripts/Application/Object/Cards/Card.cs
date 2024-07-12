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
    #region 常量
    #endregion

    #region 事件
    public event Action<Card> CardPosistionChange; //卡牌位置移动时(从手牌离开) 回收对象
    #endregion

    #region 字段
    public Player player = Player.Self; //判断召唤者是已方还是敌方
    bool m_PosState; //检测卡牌位置是否变化(从手牌移动到场上，场上移动到墓地），若为true，则发生变化
    private int m_Counter = 0; //在deckmanager场景中检测卡牌数量
    private CardInfo cardInfo; //卡牌信息
    private CardType cardType; //卡牌类型

    #endregion

    #region 属性
    //卡牌位置是否发生变化
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

    //卡牌数量记录
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

    //操控卡牌的玩家
    public Player Player 
    { 
        get => player; 
        set => player = value; 
    }

    //卡牌信息 
    public CardInfo CardInfo
    {
        get => cardInfo;
        set => cardInfo = value;
    }

    public CardType CardType { get => cardType; set => cardType = value; }
    #endregion

    #region 方法
    public void PositionChange(Card card)
    { 

    }
    #endregion

    #region Unity回调
    #endregion

    #region 事件回调
    public override void OnSpawn()
    {
        this.CardPosistionChange += PositionChange;
        this.PosState = false;
    }

    public override void OnUnspawn()
    {
        //还原脏数据
        m_Counter = 0;
        m_PosState = false;
        while (CardPosistionChange != null)
        {
            CardPosistionChange -= CardPosistionChange;
        }
    }
    #endregion

    #region 帮助方法
    #endregion
}
