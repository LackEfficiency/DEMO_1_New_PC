using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

//管理角色的 BUFF 实例，并处理 BUFF 的应用、更新和移除。
public class BuffManager : Singleton<BuffManager>
{
    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    //存放所有Buff类
    private Dictionary<string, BuffBase> m_Buffs = new Dictionary<string, BuffBase>();

    //存放所有Buff的单位
    Dictionary<MonsterCard, List<BuffInstance>> m_BuffDictionary = new Dictionary<MonsterCard, List<BuffInstance>>();

    #endregion

    #region 属性
    public Dictionary<MonsterCard, List<BuffInstance>> BuffDictionary { get => m_BuffDictionary; set => m_BuffDictionary = value; }
    public Dictionary<string, BuffBase> Buffs { get => m_Buffs; set => m_Buffs = value; }

    #endregion

    #region 方法
    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    public void Initialize()
    {
        BuffAtkIncreByDmg buffAtkIncreByDmg1 = new BuffAtkIncreByDmg("Brave1", 10, true, 1);

        //添加更多

        //添加到字典
        AddBuff(buffAtkIncreByDmg1);
    }

    public void AddBuff(BuffBase buff)
    {
        if (!Buffs.ContainsKey(buff.BuffName))
        {
            Buffs.Add(buff.BuffName, buff);
        }
    }

    public BuffBase GetBuff(string buffName)
    {
        if (Buffs.ContainsKey(buffName))
        {
            return Buffs[buffName];
        }
        return null;
    }

    public void AddBuffToMonster(MonsterCard monsterCard, BuffBase buff)
    {
        if (!BuffDictionary.ContainsKey(monsterCard))
        {
            BuffDictionary.Add(monsterCard, new List<BuffInstance>());
        }
        
        //如果buff可叠加，遍历所有buff，则添加新的buff
        if (buff.BuffStackable)
        {
            BuffInstance newBuffInstance = new BuffInstance(buff);
            newBuffInstance.Apply(monsterCard);
            BuffDictionary[monsterCard].Add(newBuffInstance);
        }   
        //如果不可叠加，找到对应的Buff，刷新持续时间
        else if (!buff.BuffStackable)
        {
            foreach (var buffInstance in BuffDictionary[monsterCard])
            {
                if (buffInstance.BuffBase.BuffName == buff.BuffName)
                {
                    buffInstance.RemainingRound = buff.BuffRound;
                }
            }
        }
    }   
    #endregion

    #region Unity回调
    #endregion

    #region 事件回调
    //在实例化Monster时订阅，单个monster行动完成时调用
    public void OnActionFinish(MonsterCard monsterCard)
    {
        //如果monsterCard有buff，遍历所有buff，调用OnActionFinish
        if (BuffDictionary.ContainsKey(monsterCard))
        {
            List<BuffInstance> buffsToRemove = new List<BuffInstance>();
            foreach (var buffInstance in BuffDictionary[monsterCard])
            {
                buffInstance.OnActionFinish(monsterCard);
                if (buffInstance.RemainingRound <= 0)
                {
                    buffsToRemove.Add(buffInstance);
                }
            }
            foreach (var buffToRemove in buffsToRemove)
            {
                BuffDictionary[monsterCard].Remove(buffToRemove);
            }
        }
    }

    //在实例化Monster时订阅，单个monster触发事件时调用
    public void TriggerEvent(MonsterCard monsterCard, BuffEvent buffEvent)
    {
        if (BuffDictionary.ContainsKey(monsterCard))
        {
            foreach (var buffInstance in BuffDictionary[monsterCard])
            {
                buffInstance.BuffBase.OnEventTriggered(monsterCard, buffEvent, buffInstance);
            }
        }
    }
    #endregion

    #region 帮助方法
    #endregion
}