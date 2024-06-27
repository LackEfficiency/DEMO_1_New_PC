using System.Collections;
using UnityEngine;

public class SkillBleed : SkillBase
{


    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    int m_BleedValue;
    #endregion

    #region 属性
    public int BleedValue { get => m_BleedValue; set => m_BleedValue = value; }

    #endregion

    #region 方法
    public SkillBleed(string skillName, int coolDown, SpellType spellType, int bleedValue) : base(skillName, coolDown, spellType)
    {
        m_BleedValue = bleedValue;
    }


    public override void Activate(MonsterCard monsterCard)
    {

    }


    #endregion

    #region Unity回调
    #endregion

    #region 事件回调
    #endregion

    #region 帮助方法
    #endregion
}