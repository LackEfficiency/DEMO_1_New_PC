using System.Collections;
using UnityEngine;

public class SkillWindWalk : SkillBase
{


    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    int m_MoveRange;
    #endregion

    #region 属性
    #endregion

    #region 方法
    public SkillWindWalk(string skillName, int coolDown, SpellType spellType, int MoveRange) : base(skillName, coolDown, spellType)
    {
        m_MoveRange = MoveRange;
    }

    public override void Activate(MonsterCard monsterCard)
    {
        BuffBase buff = Game.Instance.BuffManager.GetBuff("MoveBoost1" + m_MoveRange.ToString());
        //当buff不存在时，报错
        if (buff == null)
        {
            Debug.LogError("MoveBoost1" + m_MoveRange.ToString() + "不存在");
            return;
        }
        Game.Instance.BuffManager.AddBuffToMonster(monsterCard, buff);
    }
    #endregion

    #region Unity回调
    #endregion

    #region 事件回调
    #endregion

    #region 帮助方法
    #endregion
}