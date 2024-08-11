using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

//晕眩buff 跳过当前行动回合
public class BuffStun : BuffBase
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
    public BuffStun(string buffName, int buffRound, bool buffStackable) : base(buffName, buffRound, buffStackable)
    {

    }

    public override void ApplyBuff(MonsterCard monsterCard, BuffInstance buffInstance)
    {
        //应用时，增加不可行动标记
        monsterCard.CantAction += 1;

    }

    public override void RemoveBuff(MonsterCard monsterCard, BuffInstance buffInstance)
    {
        //移除时，减少不可行动标记
        monsterCard.CantAction -= 1;

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