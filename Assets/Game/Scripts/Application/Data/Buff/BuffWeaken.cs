using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

//具体Buff类，每次回合结束时减血
public class BuffWeaken : BuffBase
{
    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    float m_WeakenValue; //削弱攻击力的数值 在0-1之间表示百分比

    #endregion

    #region 属性
    public float WeakenValue { get => m_WeakenValue; set => m_WeakenValue = value; }
    #endregion

    #region 方法
    public BuffWeaken(string buffName, int buffRound, bool buffStackable, float WeakenValue) : base(buffName, buffRound, buffStackable)
    {
        m_WeakenValue = WeakenValue;
    }

    public override void ApplyBuff(MonsterCard monsterCard, BuffInstance buffInstance)
    {
        //应用时，降低攻击力
        if (0 < m_WeakenValue && m_WeakenValue < 1)
        {
            //百分比降低则计算当前总攻击力的百分比（base+boost）
            int weakenValue = -(int)((monsterCard.BaseAttack + monsterCard.AttackBoost) * m_WeakenValue);
            monsterCard.IncreaseDamage(weakenValue);
            //计算累计效果
            buffInstance.AccumulateEffect(weakenValue);
        }
        else if (m_WeakenValue >= 1)
        {
            int weakenValue = -(int)m_WeakenValue;
            //数值降低则直接降低数值
            monsterCard.IncreaseDamage(weakenValue);
            //计算累计效果
            buffInstance.AccumulateEffect(weakenValue);
        }
    }

    public override void RemoveBuff(MonsterCard monsterCard, BuffInstance buffInstance)
    {
        //移除时，恢复攻击力
        monsterCard.IncreaseDamage(-buffInstance.GetAccumulatedEffect());
        buffInstance.AccumulatedEffect = 0;
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