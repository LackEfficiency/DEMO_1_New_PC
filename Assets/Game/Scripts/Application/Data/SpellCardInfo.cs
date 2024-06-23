using System;
using System.Collections;
using System.Collections.Generic;


public class SpellCardInfo : CardInfo
{
    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    string m_Effect;
    SpellType m_SpellType;

    #endregion

    #region 属性
    public string Effect
    {
        get => m_Effect;
    }
    public SpellType SpellType 
    { 
        get => m_SpellType; 
    }
    #endregion

    #region 方法
    public SpellCardInfo(CardType type, int id, string name, int turn, string effect, SpellType spellType) : base(type, id, name, turn)
    {
        m_Effect = effect;
        m_SpellType = spellType;
    }
    #endregion

    #region Unity回调
    #endregion

    #region 事件回调
    #endregion

    #region 帮助方法
    #endregion
}

