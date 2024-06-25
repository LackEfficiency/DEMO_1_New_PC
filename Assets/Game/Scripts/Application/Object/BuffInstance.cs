using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

//管理 BUFF 实例的状态和累计效果。
public class BuffInstance
{
    BuffBase m_BuffBase;
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
    public void OnActionFinish(MonsterCard monsterCard)
    {
        m_RemainingRound--;
        if (m_RemainingRound <= 0)
        {
            //Buff被移除时如果要触发某个事件，则在这里调用
            m_BuffBase.RemoveBuff(monsterCard, this);
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