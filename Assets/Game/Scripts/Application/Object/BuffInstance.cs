using System.Collections;
using System;
using UnityEngine;
using UnityEngine.TextCore.Text;

//管理 BUFF 实例的状态和累计效果。
public class BuffInstance
{
    BuffBase m_BuffBase;
    //剩余回合数为-1代表永久存在
    int m_RemainingRound; 
    //buff累计改变的属性值
    int m_AccumulatedEffect;
    
    public BuffBase BuffBase { get => m_BuffBase; set => m_BuffBase = value; }
    public int RemainingRound { get => m_RemainingRound; set => m_RemainingRound = value; }
    public int AccumulatedEffect { get => m_AccumulatedEffect; set => m_AccumulatedEffect = value; }

    //初始化
    public BuffInstance(BuffBase buffbase)
    {
        this.m_BuffBase = buffbase;
        this.m_RemainingRound = buffbase.BuffRound;
        this.m_AccumulatedEffect = 0;
    }

    //添加BUFF
    public void Apply(MonsterCard monsterCard)
    {   
        //Buff被添加时如果要触发某个事件，则在这里调用
        m_BuffBase.ApplyBuff(monsterCard, this);
    }

    //行动完成调用 持续时间-1
    public void OnActionFinish(MonsterCard monsterCard, BuffInstance buffIntance)
    {
        //若不是永久存在的buff，则减少剩余回合数
        if (m_RemainingRound != -1)
        {
            m_RemainingRound--;
            if (m_RemainingRound == 0)
            {
                //Buff被移除时如果要触发某个事件，则在这里调用
                m_BuffBase.RemoveBuff(monsterCard, this);
            }
        }
        m_BuffBase.OnActionFinish(monsterCard, buffIntance);
    }

    public void OnActionStart(MonsterCard monsterCard, BuffInstance buffIntance)
    {
        m_BuffBase.OnActionStart(monsterCard, buffIntance);
    }

    //行动完成时调用
    public void OnAttack(MonsterCard monsterCard, MonsterCard target, BuffInstance buffInstance)
    {
        m_BuffBase.OnAttack(monsterCard, target, buffInstance);
    }

    //修改剩余回合数，由其他buff或者effect触发
    public void ModifyRemainingRound(int amount)
    {
        if (m_RemainingRound != -1)
        {
            m_RemainingRound = Math.Max(0, m_RemainingRound + amount);
        }
    }

    //记录修改的属性值
    public void AccumulateEffect(int amount)
    {
        AccumulatedEffect += amount;
    }

    public int GetAccumulatedEffect()
    {
        return AccumulatedEffect;
    }
}