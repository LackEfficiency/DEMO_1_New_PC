using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

//具体Buff类，每次战斗后攻击力增加
public class BuffAtkIncreByDmg : BuffBase
{
    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    int m_AtkIncre; //攻击增加值

    #endregion

    #region 属性
    public int AtkIncre { get => m_AtkIncre; set => m_AtkIncre = value; }
    #endregion

    #region 方法
    public BuffAtkIncreByDmg(string buffName, int buffRound, bool buffStackable, int atkIncre) : base(buffName, buffRound, buffStackable)
    {
        m_AtkIncre = atkIncre;
    }

    public override void ApplyBuff(MonsterCard monsterCard, BuffInstance buffInstance)
    {
       
    }

    public override void RemoveBuff(MonsterCard monsterCard, BuffInstance buffInstance)
    {
        monsterCard.IncreaseDamage(-buffInstance.GetAccumulatedEffect());
    }

    public override void UpdateBuff(MonsterCard monsterCard, BuffInstance buffInstance)
    {
        
    }

    #endregion

    #region Unity回调
    #endregion

    #region 事件回调

    public override void OnActionFinish(MonsterCard monsterCard, BuffInstance instance)
    {
        // 减少回合数的处理在BuffInStance中统一处理
        // 这里只处理其他逻辑
    }


    public override void OnEventTriggered(MonsterCard monsterCard, BuffEvent buffEvent, BuffInstance instance)
    {
        if (buffEvent == BuffEvent.OnAttack)
        {
            //每完成一次攻击，增加攻击力
            monsterCard.IncreaseDamage(m_AtkIncre);

            // 计算累计效果
            instance.AccumulateEffect(m_AtkIncre); 

        }
        //所有Buff都需要处理OnActionFinish,因此单独处理
    }


    #endregion

    #region 帮助方法
    #endregion
}