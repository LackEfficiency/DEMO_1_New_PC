using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class EffectManager : Singleton<EffectManager>
{
    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    private Dictionary<string, Effect> m_Effects = new Dictionary<string, Effect>();
    #endregion

    #region 属性
    public Dictionary<string, Effect> Effects { get => m_Effects; set => m_Effects = value; }

    #endregion

    #region 方法
    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    public void Initialize()
    {
        //初始化所有法术
        DamageEffect damageEffect1 = new DamageEffect("Damage1", "Change Hp, both positive and negative", 1);
        DamageEffect healEffect1 = new DamageEffect("Heal1", "Change Hp, both positive and negative", -1);
        DamageEffect damageEffect2 = new DamageEffect("Damage2", "Change Hp, both positive and negative", 2);
        DamageEffect healEffect2 = new DamageEffect("Heal2", "Change Hp, both positive and negative", -2);
        DamageEffect damageEffect3 = new DamageEffect("Damage3", "Change Hp, both positive and negative", 3);
        DamageEffect healEffect3 = new DamageEffect("Heal3", "Change Hp, both positive and negative", -3);
        DamageEffect damageEffect4 = new DamageEffect("Damage4", "Change Hp, both positive and negative", 4);
        DamageEffect healEffect4 = new DamageEffect("Heal4", "Change Hp, both positive and negative", -4);
        //添加更多


        //添加到字典
        AddEffect(damageEffect1);
        AddEffect(healEffect1);
        AddEffect(damageEffect2);
        AddEffect(healEffect2);
        AddEffect(damageEffect3);
        AddEffect(healEffect3);
        AddEffect(damageEffect4);
        AddEffect(healEffect4);
    }

    public void AddEffect(Effect effect)
    {
        if (!Effects.ContainsKey(effect.EffectName))
        {
            Effects.Add(effect.EffectName, effect);
        }
    }

    public Effect GetEffect(string effectName)
    {
        if (Effects.ContainsKey(effectName))
        {
            return Effects[effectName];
        }
        return null;
    }
    #endregion

    #region Unity回调
    #endregion

    #region 事件回调
    #endregion

    #region 帮助方法
    #endregion
}
