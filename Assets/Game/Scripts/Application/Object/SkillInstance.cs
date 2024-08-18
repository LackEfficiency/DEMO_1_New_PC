using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.GraphicsBuffer;

public class SkillInstance
{
    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    private SkillBase skillBase;
    private int remainCoolDown; //-1表示无CD
    private int m_AccumulatedEffect;
    private bool m_IsUsed; //一次性的技能判断是否已经使用
    private BuffAndSkillEvent skillEvent; //触发机制
    #endregion

    #region 属性
    public int RemainCoolDown { get => remainCoolDown; set => remainCoolDown = value; }
    public SkillBase SkillBase { get => skillBase; set => skillBase = value; }
    public int AccumulatdEffect { get => m_AccumulatedEffect; set => m_AccumulatedEffect = value; }
    public bool IsUsed { get => m_IsUsed; set => m_IsUsed = value; }


    #endregion

    #region 
    public SkillInstance(SkillBase skillBase)
    {
        SkillBase = skillBase;
        skillEvent = skillBase.SkillEvent;
        RemainCoolDown = Mathf.Min(skillBase.CoolDown, 0);
    }
    #endregion

    #region Unity回调

    #endregion

    #region 事件回调
    public void OnActivate(MonsterCard monsterCard, SkillInstance skillInstance)
    {
        SkillBase.Activate(monsterCard, skillInstance);
    }

    public void OnActionStart(CardActionArgs cardActionArgs, SkillInstance skillInstance)
    {
        if (RemainCoolDown <= 0 && skillEvent == BuffAndSkillEvent.OnActionStart)
        {
            SkillBase.OnActionStart(cardActionArgs, skillInstance);
            RemainCoolDown = SkillBase.CoolDown;
        }
    }

    public void OnAttack(MonsterCard attacker, MonsterCard target, SkillInstance skillInstance)
    {
        if (RemainCoolDown <= 0 && skillEvent == BuffAndSkillEvent.OnAttack)
        {
            SkillBase.OnAttack(attacker, target, skillInstance);
            RemainCoolDown = SkillBase.CoolDown;
        }
    }

    public void OnDamage(MonsterCard attacker, MonsterCard target, SkillInstance skillInstance)
    {
        if (RemainCoolDown <= 0 && skillEvent == BuffAndSkillEvent.OnDamage)
        {
            SkillBase.OnDamage(attacker, target, skillInstance);
            RemainCoolDown = SkillBase.CoolDown;
        }
    }

    public void OnActionFinish(CardActionArgs cardActionArgs, SkillInstance skillInstance)
    {
        if (RemainCoolDown <= 0 && skillEvent == BuffAndSkillEvent.OnActionFinish)
        {
            SkillBase.OnActionFinish(cardActionArgs, skillInstance);
            RemainCoolDown = SkillBase.CoolDown;
        }
    }
    #endregion

    #region 帮助方法
    #endregion




}