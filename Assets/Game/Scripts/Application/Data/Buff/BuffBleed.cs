using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

//具体Buff类，每次回合结束时减血
public class BuffBleed : BuffBase
{
    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    int m_BleedValue; //流血伤害

    #endregion

    #region 属性
    public int BleedValue { get => m_BleedValue; set => m_BleedValue = value; }
    #endregion

    #region 方法
    public BuffBleed(string buffName, int buffRound, bool buffStackable, int bleedValue) : base(buffName, buffRound, buffStackable)
    {
        m_BleedValue = bleedValue;
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

    public override void OnActionFinish(CardActionArgs cardActionArgs, BuffInstance instance)
    {

         //回合结束扣血
         MonsterCard monsterCard = cardActionArgs.self;
         monsterCard.Damage(null, BleedValue);

    }


    #endregion

    #region 帮助方法
    #endregion
}