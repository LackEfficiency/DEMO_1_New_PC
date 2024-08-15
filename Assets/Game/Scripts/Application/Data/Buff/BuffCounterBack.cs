using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

//反击buff 收到伤害时反击
public class BuffCounterBack : BuffBase
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
    public BuffCounterBack(string buffName, int buffRound, bool buffStackable) : base(buffName, buffRound, buffStackable)
    {

    }
    public override void ApplyBuff(MonsterCard monsterCard, BuffInstance buffInstance)
    {

    }

    public override void RemoveBuff(MonsterCard monsterCard, BuffInstance buffInstance)
    {

    }

    public override void UpdateBuff(MonsterCard monsterCard, BuffInstance buffInstance)
    {

    }

    #endregion

    #region Unity回调
    #endregion

    #region 事件回调
    public override void OnTakeDamage(MonsterCard attacker, MonsterCard self, BuffInstance instance)
    {
        //只有明确来源的伤害才会触发反击
        if (attacker != null)
        {
            self.Attack(self, attacker);
        }
        
    }


    #endregion

    #region 帮助方法
    #endregion
}