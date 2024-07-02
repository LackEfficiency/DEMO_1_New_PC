using System;
using System.Collections;
using UnityEngine;

public class Summoner : MonsterCard
{

    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    private Player m_Player; //玩家
    private int m_RemainingCards; //剩余卡牌
    private int m_HandCards = 0; //手牌数量
    #endregion

    #region 属性
    public int RemainingCards
    {
        get => m_RemainingCards;
        set
        {
            //范围约定
            value = Mathf.Clamp(value, 0, 40);
            m_RemainingCards = value;

            //状态变化事件
            OnStatusChanged(this);
        }
    }

    public int HandCards
    {
        get => m_HandCards;
        set
        {
            //范围约定
            value = Mathf.Clamp(value, 0, 7);
            m_HandCards = value;

            //状态变化事件
            OnStatusChanged(this);
        }
    }

    public Player Player1 { get => m_Player; set => m_Player = value; }
    #endregion

    #region 方法
    #endregion

    #region Unity回调
    #endregion

    #region 事件回调

    public override void OnSpawn()
    {
        base.OnSpawn();
    }

    public override void OnUnspawn()
    {
        base.OnUnspawn();
    }
    #endregion

    #region 帮助方法
    #endregion
}