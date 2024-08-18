using System.Collections;
using UnityEngine;

public class SkillStun : SkillBase
{


    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    int m_StunValue;
    #endregion

    #region 属性
    #endregion

    #region 方法
    public SkillStun(string skillName, int coolDown, SpellType spellType, BuffAndSkillEvent skillEvent, int stunValue) : base(skillName, coolDown, spellType, skillEvent)
    {
        m_StunValue = stunValue;
    }

    public override void Activate(MonsterCard monsterCard, SkillInstance skillInstance)
    {

    }

    public override void OnAttack(MonsterCard monsterCard, MonsterCard target, SkillInstance skillInstance)
    {
        BuffBase buff = Game.Instance.BuffManager.GetBuff("Stun" + m_StunValue.ToString());
        //当buff不存在时，报错
        if (buff == null)
        {
            Debug.LogError("Stun" + m_StunValue.ToString() + "不存在");
            return;
        }
        Game.Instance.BuffManager.AddBuffToMonster(target, buff);
    }
    #endregion

    #region Unity回调
    #endregion

    #region 事件回调
    #endregion

    #region 帮助方法
    #endregion
}