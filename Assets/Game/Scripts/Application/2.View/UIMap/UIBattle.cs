using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIBattle : View
{

    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    MapBattle m_Map = null;

    public Button btnClose;
    public Button btnStart;

    int m_SelectedIndex = 0; //TODO: 暂时设为0

    GameModel gm = null;
    #endregion

    #region 属性
    public override string Name
    {
        get { return Consts.V_Battle; }
    }

    #endregion

    #region 方法
    //点击图标时调用Show
    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    //点击关闭
    public void Close()
    {
        this.gameObject.SetActive(false);
        gm.IsPlaying = true;
    }
    #endregion

    #region Unity回调
    #endregion

    #region 事件回调
    //关心进入当前场景，从而调用HandleEvent
    public override void RegisterEvents()
    {
        AttentionEvents.Add(Consts.E_EnterScene);
        AttentionEvents.Add(Consts.E_ShowBattlePanel);
    }

    //进入场景需要初始化关卡列表
    public override void HandleEvent(string eventName, object data = null)
    {
        switch (eventName)
        {
            case Consts.E_EnterScene:
                SceneArgs e = data as SceneArgs;
                if (e.SceneIndex == 2)
                {
                    //获取数据模型
                    gm = GetModel<GameModel>();
                }
                break;
            case Consts.E_ShowBattlePanel:
                Show();
                gm.IsPlaying = false;
                break;
        }
    }

    public void onCloseClick()
    {
        Close();
    }

    public void onStartClick()
    {
        StartLevelArgs e = new StartLevelArgs()
        {
            LevelID = m_SelectedIndex,
        };

        SendEvent(Consts.E_StartLevel, e);
    }
    #endregion

    #region 帮助方法
    #endregion
}
