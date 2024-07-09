using System.Collections;
using UnityEngine;

public class SkillWeaken : SkillBase
{

    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    float m_WeakenValue; //减低攻击力的数值 可以为固定值 若值在0-1之间，为百分比
    #endregion

    #region 属性
    public float WeakenValue { get => m_WeakenValue; set => m_WeakenValue = value; }
    #endregion

    #region 方法
    public SkillWeaken(string skillName, int coolDown, SpellType spellType, float weakenValue) : base(skillName, coolDown, spellType)
    {
        m_WeakenValue = weakenValue;
    }

    public override void OnAttack(MonsterCard monsterCard, MonsterCard target)
    {
        //命名没有.的字符串
        BuffBase buff = Game.Instance.BuffManager.GetBuff("Weaken"+ m_WeakenValue.ToString().Replace(".", ""));
        //当buff不存在时，报错
        if (buff == null)
        {
            Debug.LogError("Weaken" + m_WeakenValue.ToString() + "不存在");
            return;
        }
        Game.Instance.BuffManager.AddBuffToMonster(target, buff);
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