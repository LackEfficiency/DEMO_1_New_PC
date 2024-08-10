using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

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
        LoadEffectConfig();
    }

    //加载Effect
    private void LoadEffectConfig()
    {
        string json = File.ReadAllText(Consts.EffectDataDir);
        EffectConfig effectConfig = JsonUtility.FromJson<EffectConfig>(json);
        foreach (var effectData in effectConfig.effects)
        {
            Effect effect = CreateEffectFromData(effectData);
            if (effect != null)
            {
                AddEffect(effect);
            }
        }
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
    //Effect工厂
    private Effect CreateEffectFromData(EffectData data)
    {
        switch (data.type)
        {
            case "DamageEffect":
                return new DamageEffect(data.name, data.description, data.value);
            // 添加其他效果类型
            default:
                Debug.LogWarning("Unknown Effect type: " + data.type);
                return null;
        }
    }

    #endregion
}
