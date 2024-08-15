using System.Collections;
using UnityEngine;

public class SkillCounterBack : SkillBase
{


    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    #endregion

    #region 属性
    #endregion

    #region 方法
    public SkillCounterBack(string skillName, int coolDown, SpellType spellType) : base(skillName, coolDown, spellType)
    {

    }


    public override void Activate(MonsterCard monsterCard)
    {
        BuffBase buff = Game.Instance.BuffManager.GetBuff("CounterBack" + "FromSkill");
        //当buff不存在时，报错
        if (buff == null)
        {
            Debug.LogError("CounterBack" + "FromSkill" + "不存在");
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