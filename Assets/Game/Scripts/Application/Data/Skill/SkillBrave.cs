using System.Collections;
using UnityEngine;

public class SkillBrave : SkillBase
{

    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    int m_BraveValue;
    #endregion

    #region 属性
    public int BraveValue { get => m_BraveValue; set => m_BraveValue = value; }
    #endregion

    #region 方法
    public SkillBrave(string skillName, int coolDown, SpellType spellType, BuffAndSkillEvent skillEvent, int braveValue) : base(skillName, coolDown, spellType, skillEvent)
    {
        m_BraveValue = braveValue;
    }



    public override void Activate(MonsterCard monsterCard, SkillInstance skillInstance)
    {
        BuffBase buff = Game.Instance.BuffManager.GetBuff("Brave"+ m_BraveValue.ToString()+ "FromSkill");
        //当buff不存在时，报错
        if (buff == null)
        {
            Debug.LogError("Brave" + m_BraveValue.ToString() + "FromSkill" + "不存在");
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