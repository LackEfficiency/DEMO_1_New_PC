using System.Collections;
using UnityEngine;

public class SkillHealAnAlly : SkillBase
{


    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    int m_HealValue;
    #endregion

    #region 属性
    #endregion

    #region 方法
    public SkillHealAnAlly(string skillName, int coolDown, SpellType spellType, int healValue) : base(skillName, coolDown, spellType)
    {
        m_HealValue = healValue;
    }

    public override void Activate(MonsterCard monsterCard)
    {

    }

    public override void OnActionStart(CardActionArgs cardActionArgs)
    {
        MonsterCard target = Tools.GetLowestHp(cardActionArgs.selfCardsOnField);
        target.Heal(cardActionArgs.self, m_HealValue);
    }
    #endregion

    #region Unity回调
    #endregion

    #region 事件回调
    #endregion

    #region 帮助方法
    #endregion

}