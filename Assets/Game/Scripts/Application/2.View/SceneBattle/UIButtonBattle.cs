using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIButtonBattle : View
{
    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    public Button btnTurnEnd; //当前阶段结束按钮
    public Button btnRemoveUnit; //移除单位按钮
    #endregion

    #region 属性
    public override string Name
    {
        get { return Consts.V_ButtonBattle; }
    }
    #endregion

    #region 方法
    #endregion

    #region Unity回调
    public void OnClickEnd()
    {
        SendEvent(Consts.E_TurnEnd);
    }
    #endregion

    #region 事件回调
    public override void RegisterEvents()
    {

    }
    public override void HandleEvent(string eventName, object data = null)
    {

    }
    #endregion

    #region 帮助方法
    #endregion
}
