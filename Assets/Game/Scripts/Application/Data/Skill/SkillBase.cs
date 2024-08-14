using System.Collections;
using UnityEngine;

public abstract class SkillBase
{
    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    private string m_SkillName;
    private int m_CoolDown; //-1代表无CD 100代表一次性
    private SpellType m_spellType; //技能对象

    #endregion

    #region 属性
    public string SkillName { get => m_SkillName; set => m_SkillName = value; }
    public int CoolDown { get => m_CoolDown; set => m_CoolDown = value; }
    public SpellType SpellType { get => m_spellType; set => m_spellType = value; }

    #endregion

    #region 方法
    protected SkillBase(string skillName, int coolDown, SpellType spellType)
    {
        m_SkillName = skillName;
        m_CoolDown = coolDown;
        m_spellType = spellType;
    }

    //被动技能，实例化Card时就会调用，通常是添加一个Buff或赋予单次效果，无CD技能全部写在这里
    public abstract void Activate(MonsterCard monsterCard);

    //有CD技能

    //攻击时触发的技能，通常是造成伤害追加伤害或者添加一个DeBuff
    public virtual void OnAttack(MonsterCard monsterCard, MonsterCard target) { }

    //行动开始时触发的技能，例如治愈一名队友或者造成一次伤害
    public virtual void OnActionStart(MonsterCard monsterCard) { }

    //行动结束时触发的技能，例如回复自己生命值或者造成一次伤害
    public virtual void OnActionFinish(MonsterCard monsterCard) { }

    #endregion

    #region Unity回调
    #endregion

    #region 事件回调
    #endregion

    #region 帮助方法
    #endregion
}