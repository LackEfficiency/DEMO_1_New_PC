using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class EffectManager : MonoBehaviour
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
