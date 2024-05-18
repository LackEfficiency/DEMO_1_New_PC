using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpellCard : Card
{
    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    string m_Effect;

    #endregion

    #region 属性
    public string Effect 
    { 
        get => m_Effect; 
    }
    #endregion

    #region 方法
    #endregion

    #region Unity回调
    #endregion

    #region 事件回调
    public override void OnSpawn()
    {
        base.OnSpawn();
    }

    public override void OnUnspawn()
    {
        base.OnUnspawn();
    }
    #endregion

    #region 帮助方法
    #endregion
}
