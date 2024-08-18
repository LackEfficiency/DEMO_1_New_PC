using System.Collections;
using UnityEngine;

public class SkillInitialImpact : SkillBase
{


    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    int m_AtkIncreaseRate;
    #endregion

    #region 属性
    #endregion

    #region 方法
    public SkillInitialImpact(string skillName, int coolDown, SpellType spellType, BuffAndSkillEvent skillEvent, int AtkIncreaseRate) : base(skillName, coolDown, spellType, skillEvent)
    {
        m_AtkIncreaseRate = AtkIncreaseRate;
    }

    public override void Activate(MonsterCard monsterCard, SkillInstance skillInstance)
    {
        //添加时 目标卡牌的攻击力增加 m_AtkIncreaseRate * 初始攻击力
        skillInstance.AccumulatdEffect += m_AtkIncreaseRate * monsterCard.BaseAttack;
        monsterCard.AttackBoost += skillInstance.AccumulatdEffect;
    }

    public override void OnDamage(MonsterCard monsterCard, MonsterCard target, SkillInstance skillInstance)
    {
        //首次攻击后，攻击力回到正常
        monsterCard.AttackBoost -= skillInstance.AccumulatdEffect;
        skillInstance.AccumulatdEffect = 0;
    }
    #endregion

    #region Unity回调
    #endregion

    #region 事件回调
    #endregion

    #region 帮助方法
    #endregion
}