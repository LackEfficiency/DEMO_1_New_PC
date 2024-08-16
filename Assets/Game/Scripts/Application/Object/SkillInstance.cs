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
    #endregion

    #region 属性
    public int RemainCoolDown { get => remainCoolDown; set => remainCoolDown = value; }
    public SkillBase SkillBase { get => skillBase; set => skillBase = value; }

    #endregion

    #region 
    public SkillInstance(SkillBase skillBase)
    {
        SkillBase = skillBase;
        RemainCoolDown = Mathf.Min(skillBase.CoolDown, 0);
    }
    #endregion

    #region Unity回调

    #endregion

    #region 事件回调
    public void OnActivate(MonsterCard monsterCard)
    {
        SkillBase.Activate(monsterCard);
    }

    public void OnActionStart(CardActionArgs cardActionArgs)
    {
        if (RemainCoolDown <= 0)
        {
            SkillBase.OnActionStart(cardActionArgs);
            RemainCoolDown = SkillBase.CoolDown;
        }
    }

    public void OnAttack(MonsterCard monsterCard, MonsterCard target)
    {
        if (RemainCoolDown <= 0)
        {
            SkillBase.OnAttack(monsterCard, target);
            RemainCoolDown = SkillBase.CoolDown;
        }
    }

    public void OnActionFinish(CardActionArgs cardActionArgs)
    {
        if (RemainCoolDown <= 0)
        {
            SkillBase.OnActionFinish(cardActionArgs);
            RemainCoolDown = SkillBase.CoolDown;
        }
    }
    #endregion

    #region 帮助方法
    #endregion




}