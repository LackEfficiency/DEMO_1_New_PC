using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SkillManager : Singleton<SkillManager>
{

    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    //存放所有Skill类
    private Dictionary<string, SkillBase> m_Skills = new Dictionary<string, SkillBase>();

    //存放所有Skill的单位
    Dictionary<MonsterCard, List<SkillInstance>> m_SkillDictionary = new Dictionary<MonsterCard, List<SkillInstance>>();
    #endregion

    #region 属性
    public Dictionary<string, SkillBase> Skills { get => m_Skills; set => m_Skills = value; }
    public Dictionary<MonsterCard, List<SkillInstance>> SkillDictionary { get => m_SkillDictionary; set => m_SkillDictionary = value; }
    #endregion

    #region 方法
    protected override void Awake()
    {
        base.Awake();
        LoadSkillConfig();
    }

    //加载技能配置
    private void LoadSkillConfig()
    {
        string json = File.ReadAllText(Consts.SkillDataDir);
        SkillConfig skillConfig = JsonUtility.FromJson<SkillConfig>(json);
        foreach (var skillData in skillConfig.skills)
        {
            SkillBase skill = CreateSkillFromData(skillData);
            if (skill != null)
            {
                AddSkill(skill);
            }
        }
    }

    public void AddSkill(SkillBase skill)
    {
        if (!Skills.ContainsKey(skill.SkillName))
        {
            Skills.Add(skill.SkillName, skill);
        }
    }

    public SkillBase GetSkill(string skillName)
    {
        if (Skills.ContainsKey(skillName))
        {
            return Skills[skillName];
        }
        return null;
    }

    public void AddSkillToMonster(MonsterCard monsterCard, SkillBase skill)
    {
        if (!SkillDictionary.ContainsKey(monsterCard))
        {
            SkillDictionary.Add(monsterCard, new List<SkillInstance>());
        }

        //不添加重复技能
        foreach (var skillInstance in SkillDictionary[monsterCard])
        {
            if (skillInstance.SkillBase.SkillName == skill.SkillName)
            {
                return;
            }
        }

        SkillInstance newSkillInstance = new SkillInstance(skill);
        SkillDictionary[monsterCard].Add(newSkillInstance);
        newSkillInstance.OnActivate(monsterCard);
    }

    //传入卡牌 获取身上的所有技能
    public List<SkillInstance> GetSkillOnCard(MonsterCard monsterCard)
    {
        if (SkillDictionary.ContainsKey(monsterCard))
        {
            return SkillDictionary[monsterCard];
        }
        return null;
    }

    //移除所有技能
    public void RemoveAllSkills(MonsterCard monsterCard)
    {
        if (!SkillDictionary.ContainsKey(monsterCard))
        {
            return;
        }
        //移除这个单位的所有技能
        List<SkillInstance> skillsToRemove = new List<SkillInstance>();

        foreach (var skillInstance in SkillDictionary[monsterCard])
        {
            skillsToRemove.Add(skillInstance);
        }
        foreach (var skillInstance in skillsToRemove)
        {
            SkillDictionary[monsterCard].Remove(skillInstance);
        }
        //移除这个卡
        SkillDictionary.Remove(monsterCard);
    }
    #endregion

    #region Unity回调
    #endregion

    #region 事件回调
    public void OnActionStart(CardActionArgs cardActionArgs)
    {
        MonsterCard monsterCard = cardActionArgs.self;
        if (SkillDictionary.ContainsKey(monsterCard))
        {
            foreach (var skillInstance in SkillDictionary[monsterCard])
            {
                skillInstance.OnActionStart(cardActionArgs);
            }
        }
    }

    public void OnAttack(MonsterCard monsterCard, MonsterCard monsterCard2)
    {
        if (SkillDictionary.ContainsKey(monsterCard))
        {
            foreach (var skillInstance in SkillDictionary[monsterCard])
            {
                skillInstance.OnAttack(monsterCard, monsterCard2);
            }
        }
    }

    public void OnActionFinish(CardActionArgs cardActionArgs)
    {
        MonsterCard monsterCard = cardActionArgs.self;
        if (SkillDictionary.ContainsKey(monsterCard))
        {
            foreach (var skillInstance in SkillDictionary[monsterCard])
            {
                skillInstance.OnActionFinish(cardActionArgs);
            }
        }
    }
    #endregion

    #region 帮助方法
    //Skill工厂
    private SkillBase CreateSkillFromData(SkillData data)
    {
        // 根据类型创建不同的技能实例
        switch (data.type)
        {
            case "SkillBrave":
                return new SkillBrave(data.name, data.coolDown, Enum.Parse<SpellType>(data.spellType), (int)data.value);
            case "SkillBleed":
                return new SkillBleed(data.name, data.coolDown, Enum.Parse<SpellType>(data.spellType), (int)data.value);
            case "SkillWeaken":
                return new SkillWeaken(data.name, data.coolDown, Enum.Parse<SpellType>(data.spellType), data.value);
            case "SkillGuardian":
                return new SkillGuardian(data.name, data.coolDown, Enum.Parse<SpellType>(data.spellType));
            case "SkillStun":
                return new SkillStun(data.name, data.coolDown, Enum.Parse<SpellType>(data.spellType), (int)data.value);
            case "SkillWindWalk":
                return new SkillWindWalk(data.name, data.coolDown, Enum.Parse<SpellType>(data.spellType), (int)data.value);
            case "SkillCounterBack":
                return new SkillCounterBack(data.name, data.coolDown, Enum.Parse<SpellType>(data.spellType));
            case "SkillHealAnAlly":
                return new SkillHealAnAlly(data.name, data.coolDown, Enum.Parse<SpellType>(data.spellType), (int)data.value);
            // 添加其他技能类型
            default:
                Debug.LogWarning("Unknown Skill type: " + data.type);
                return null;
        }
    }
    #endregion
}