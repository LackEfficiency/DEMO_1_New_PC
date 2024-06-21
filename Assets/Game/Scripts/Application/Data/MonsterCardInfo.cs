using System;
using System.Collections;
using System.Collections.Generic;


public class MonsterCardInfo : CardInfo
{
    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    int m_HP;
    int m_MaxHP;
    int m_Attack;
    int m_AttackRange = 2;
    int m_MoveRange = 2;    

    #endregion

    #region 属性
    public int HP
    {
        get => m_HP;
    }
    public int Attack
    {
        get => m_Attack;
    }
    public int MaxHP
    {
        get => m_MaxHP;
    }
    public int AttackRange 
    { 
        get => m_AttackRange; 
        set => m_AttackRange = value; 
    }
    public int MoveRange 
    { 
        get => m_MoveRange; 
        set => m_MoveRange = value; 
    }
    #endregion

    #region 方法
    public MonsterCardInfo(CardType type, int id, string name, int cost, int attack, int maxHp) : base(type, id, name, cost)
    {
        m_HP = maxHp;
        m_MaxHP = maxHp;
        m_Attack = attack;
    }
    #endregion

    #region Unity回调
    #endregion

    #region 事件回调
    #endregion

    #region 帮助方法
    #endregion
}

