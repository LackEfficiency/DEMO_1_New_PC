using System.Collections;
using UnityEngine;

//Buff基类
public abstract class BuffBase 
{
    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    string m_BuffName; //buff名称   
    int m_BuffRound; //buff持续回合
    bool m_BuffStackable; //buff是否可叠加
    #endregion

    #region 属性
    public string BuffName { get => m_BuffName; set => m_BuffName = value; }
    public int BuffRound { get => m_BuffRound; set => m_BuffRound = value; }
    public bool BuffStackable { get => m_BuffStackable; set => m_BuffStackable = value; }
    #endregion

    #region 方法
    protected BuffBase(string buffName, int buffRound, bool buffStackable)
    {
        m_BuffName = buffName;
        m_BuffRound = buffRound;
        m_BuffStackable = buffStackable;
    }

    //添加buff时调用
    public abstract void ApplyBuff(MonsterCard monsterCard, BuffInstance buffInstance);

    //移除buff时调用
    public abstract void RemoveBuff(MonsterCard monsterCard, BuffInstance buffInstance);

    //更新buff时调用
    public abstract void UpdateBuff(MonsterCard monsterCard, BuffInstance buffInstance);
    #endregion

    #region Unity回调
    #endregion

    #region 事件回调
    //buff触发机制
    //public abstract void OnEventTriggered(MonsterCard monsterCard, BuffEvent buffEvent, BuffInstance instance);

    public virtual void OnAttack(MonsterCard monsterCard, MonsterCard target, BuffInstance instance) { }

    public virtual void OnActionFinish(CardActionArgs cardActionArgs, BuffInstance instance) { }

    public virtual void OnActionStart(CardActionArgs cardActionArgs, BuffInstance instance) { }

    public virtual void OnTakeDamage(MonsterCard attacker, MonsterCard self, BuffInstance instance) { }

    public virtual void OnDamage(MonsterCard attacker, MonsterCard self, BuffInstance instance) { }


    #endregion

    #region 帮助方法
    #endregion
}