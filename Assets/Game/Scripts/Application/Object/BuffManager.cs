using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    private readonly object buffDictionaryLock = new object();

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
        LoadBuffConfig();
    }


    //通过配置文件加载Buff
    private void LoadBuffConfig()
    {
        string json = File.ReadAllText(Consts.BuffDataDir);
        BuffConfig buffConfig = JsonUtility.FromJson<BuffConfig>(json);
        foreach (var buffData in buffConfig.buffs)
        {
            BuffBase buff = CreateBuffFromConfig(buffData);
            if (buff != null)
            {
                AddBuff(buff);
            }
        }
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
        lock (buffDictionaryLock)
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
            //如果不可叠加，先检查是否存在相应Buff,若找到对应的Buff，刷新持续时间
            //若找不到，则添加新的Buff
            else if (!buff.BuffStackable)
            {
                bool find = false;
                foreach (var buffInstance in BuffDictionary[monsterCard])
                {

                    if (buffInstance.BuffBase.BuffName == buff.BuffName)
                    {
                        buffInstance.RemainingRound = buff.BuffRound;
                        find = true;
                    }
                }
                if (find == false)
                {
                    BuffInstance newBuffInstance = new BuffInstance(buff);
                    newBuffInstance.Apply(monsterCard);
                    BuffDictionary[monsterCard].Add(newBuffInstance);
                }
            }
        }

    }

    //移除所有buff
    public void RemoveAllBuff(MonsterCard monsterCard)
    {
        lock (buffDictionaryLock)
        {
            if (!BuffDictionary.ContainsKey(monsterCard))
            {
                return;
            }
            List<BuffInstance> buffsToRemove = new List<BuffInstance>();
            //移除这个单位的所有buff
            foreach (var buffInstance in BuffDictionary[monsterCard])
            {
                buffsToRemove.Add(buffInstance);
            }
            foreach (var buffToRemove in buffsToRemove)
            {
                BuffDictionary[monsterCard].Remove(buffToRemove);
            }
            //移除这个卡
            BuffDictionary.Remove(monsterCard);
        }
    }

    //传入卡牌 获取身上的所有buff
    public List<BuffInstance> GetBuffOnCard(MonsterCard monsterCard)
    {
        lock (buffDictionaryLock)
        {
            if (BuffDictionary.ContainsKey(monsterCard))
            {
                return BuffDictionary[monsterCard];
            }
            return null;
        }
    }
    #endregion

    #region Unity回调
    #endregion

    #region 事件回调
    //在实例化Monster时订阅，单个monster触发事件时调用
    //public void TriggerEvent(MonsterCard monsterCard, BuffEvent buffEvent)
    //{
    //    if (BuffDictionary.ContainsKey(monsterCard))
    //    {
    //        foreach (var buffInstance in BuffDictionary[monsterCard])
    //        {
    //            buffInstance.BuffBase.OnEventTriggered(monsterCard, buffEvent, buffInstance);
    //        }
    //    }
    //}
    public void OnActionFinish(CardActionArgs cardActionArgs)
    {
        MonsterCard monsterCard = cardActionArgs.self;
        lock (buffDictionaryLock)
        {
            // 如果 monsterCard 有 buff，遍历所有 buff，调用 OnActionFinish
            if (BuffDictionary.ContainsKey(monsterCard))
            {
                List<BuffInstance> buffsToRemove = new List<BuffInstance>();
                var buffs = BuffDictionary[monsterCard];
                foreach (var buffInstance in buffs)
                {
                    buffInstance.OnActionFinish(cardActionArgs, buffInstance);
                    if (buffInstance.RemainingRound == 0)
                    {
                        buffsToRemove.Add(buffInstance);
                    }
                }

                // 在遍历完成后实际修改集合
                foreach (var buffToRemove in buffsToRemove)
                {
                    buffs.Remove(buffToRemove);
                }
            }
        }
    }


    public void OnActionStart(CardActionArgs cardActionArgs)
    {
        MonsterCard monsterCard = cardActionArgs.self;
        if (BuffDictionary.ContainsKey(monsterCard))
        {
            foreach (var buffInstance in BuffDictionary[monsterCard])
            {
                buffInstance.OnActionStart(cardActionArgs, buffInstance);
            }
        }
    }

    public void OnAttack(MonsterCard monsterCard, MonsterCard target)
    {
        if (BuffDictionary.ContainsKey(monsterCard))
        {
            foreach (var buffInstance in BuffDictionary[monsterCard])
            {
                buffInstance.OnAttack(monsterCard, target, buffInstance);
            }
        }
    }

    public void OnTakeDamage(MonsterCard attacker, MonsterCard self)
    {
        if (BuffDictionary.ContainsKey(self))
        {
            foreach (var buffInstance in BuffDictionary[self])
            {
                buffInstance.OnTakeDamage(attacker, self, buffInstance);
            }
        }
    }


    #endregion

    #region 帮助方法
    //Buff工厂
    private BuffBase CreateBuffFromConfig(BuffData data)
    {
        switch (data.type)
        {
            case "BuffAtkIncreByDmg":
                return new BuffAtkIncreByDmg(data.name, data.duration, data.stackable, (int)data.value);
            case "BuffBleed":
                return new BuffBleed(data.name, data.duration, data.stackable, (int)data.value);
            case "BuffWeaken":
                return new BuffWeaken(data.name, data.duration, data.stackable, data.value);
            case "BuffGuardian":
                return new BuffGuardian(data.name, data.duration, data.stackable);
            case "BuffStun":
                return new BuffStun(data.name, data.duration, data.stackable);
            case "BuffMoveBoost":
                return new BuffMoveBoost(data.name, data.duration, data.stackable, (int)data.value);
            case "BuffCounterBack":
                return new BuffCounterBack(data.name, data.duration, data.stackable);
            default:
                Debug.LogWarning("Unknown Buff type: " + data.type);
                return null;
        }
    }
    #endregion
}