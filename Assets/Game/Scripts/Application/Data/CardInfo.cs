using System;
using System.Collections;
using System.Collections.Generic;


public class CardInfo
{
    #region 常量
    #endregion

    #region 事件
    public event Action DataChanged;
    #endregion

    #region 字段
    public CardType m_CardType; //卡牌类型
    public int m_CardID;
    public string m_CardName;
    public int m_Cost;
    #endregion

    #region 属性
    public CardInfo(CardType type, int id, string name, int cost)
    {
        m_CardType = type;
        m_CardID = id;
        m_CardName = name;
        m_Cost = cost;
    }

    public int CardID
    {
        get => m_CardID;
    }

    public string CardName
    {
        get => m_CardName;
    }

    public int Cost
    {
        get => m_Cost;
        set
        {
            if (m_Cost != value)
            {
                m_Cost = value;
                OnDataChanged();
            }
        }
    }

    public CardType CardType
    {
        get => m_CardType;
    }

    #endregion

    #region 方法
    private void OnDataChanged()
    {
        DataChanged?.Invoke();
    }
    #endregion

    #region Unity回调
    #endregion

    #region 事件回调
    #endregion

    #region 帮助方法
    #endregion
}

