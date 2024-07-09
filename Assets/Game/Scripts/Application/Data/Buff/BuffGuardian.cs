using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

//守护Buff 优先攻击身后的敌人 且攻击目标未消灭时，不会移动
public class BuffGuardian : BuffBase
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
    public BuffGuardian(string buffName, int buffRound, bool buffStackable) : base(buffName, buffRound, buffStackable)
    {

    }

    public override void ApplyBuff(MonsterCard monsterCard, BuffInstance buffInstance)
    {
        //应用时，提高守护值
        monsterCard.IsGuardian += 1;

    }

    public override void RemoveBuff(MonsterCard monsterCard, BuffInstance buffInstance)
    {
        //移除时，减少守护值
        monsterCard.IsGuardian -= 1;

    }

    public override void UpdateBuff(MonsterCard monsterCard, BuffInstance buffInstance)
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