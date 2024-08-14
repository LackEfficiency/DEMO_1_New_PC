using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

//风行Buff 移动距离提高
public class BuffMoveBoost : BuffBase
{
    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    int m_MoveRange; //移动距离提高的数值
    #endregion

    #region 属性
    #endregion

    #region 方法
    public BuffMoveBoost(string buffName, int buffRound, bool buffStackable, int MoveRange) : base(buffName, buffRound, buffStackable)
    {
        m_MoveRange = MoveRange;
    }

    public override void ApplyBuff(MonsterCard monsterCard, BuffInstance buffInstance)
    {
        //应用时，提高移动距离
        monsterCard.MoveRange += m_MoveRange;

    }

    public override void RemoveBuff(MonsterCard monsterCard, BuffInstance buffInstance)
    {
        //移除时，减少移动距离
        monsterCard.MoveRange -= m_MoveRange;

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