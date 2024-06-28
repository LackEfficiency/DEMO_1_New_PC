using System;
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
        Initialize();
    }

    public void Initialize()
    {
        SkillBrave skillBrave1 = new SkillBrave("Brave1", -1, SpellType.Self, 1);
        //添加更多

        //添加到字典
        AddSkill(skillBrave1);
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
    #endregion

    #region Unity回调
    #endregion

    #region 事件回调
    public void OnActionStart(MonsterCard monsterCard)
    {
        if (SkillDictionary.ContainsKey(monsterCard))
        {
            foreach (var skillInstance in SkillDictionary[monsterCard])
            {
                skillInstance.OnActionStart(monsterCard);
            }
        }
    }

    public void OnAttack(MonsterCard monsterCard)
    {
        if (SkillDictionary.ContainsKey(monsterCard))
        {
            foreach (var skillInstance in SkillDictionary[monsterCard])
            {
                skillInstance.OnAttack(monsterCard);
            }
        }
    }

    public void OnActionFinish(MonsterCard monsterCard)
    {
        if (SkillDictionary.ContainsKey(monsterCard))
        {
            foreach (var skillInstance in SkillDictionary[monsterCard])
            {
                skillInstance.OnActionFinish(monsterCard);
            }
        }
    }
    #endregion

    #region 帮助方法
    #endregion
}