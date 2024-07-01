using System;
using System.Collections;
using UnityEngine;

public class Summoner : Reusable, IReusable
{

    #region 常量
    #endregion

    #region 事件
    public event Action<Summoner> Dead; //死亡
    public event Action<Summoner> StatusChanged; //状态更新
    #endregion

    #region 字段
    private Player m_Player; //玩家
    private int m_Hp; //当前血量
    private int m_RemainingCards; //剩余卡牌
    private int m_HandCards = 0; //手牌数量
    #endregion

    #region 属性
    public int Hp
    {
        get => m_Hp;
        set
        {
            //范围约定
            value = Mathf.Clamp(value, 0, 100);
            m_Hp = value;

            //血量变化事件
            if (StatusChanged != null)
                StatusChanged(this);

            //死亡事件
            if(m_Hp == 0)
            {
                if(Dead != null)
                {
                    Dead(this);
                }   
            }
        }
    }

    public int RemainingCards
    {
        get => m_RemainingCards;
        set
        {
            //范围约定
            value = Mathf.Clamp(value, 0, 40);
            m_RemainingCards = value;

            //状态变化事件
            if (StatusChanged != null)
                StatusChanged(this);
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
            if (StatusChanged != null)
                StatusChanged(this);
        }
    }

    public Player Player { get => m_Player; set => m_Player = value; }
    #endregion

    #region 方法
    protected void Die(Summoner summoner)
    {
        Game.Instance.ObjectPool.Unspawn(this.gameObject);
    }
    #endregion

    #region Unity回调
    #endregion

    #region 事件回调

    public override void OnSpawn()
    {
        this.StatusChanged += StatusChanged;
        this.Dead += Die;
    }

    public override void OnUnspawn()
    {
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