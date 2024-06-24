using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIStore : View
{
    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段

    //用游戏模型储存卡牌信息 
    GameModel m_GameModel;
    #endregion

    #region 属性
    public override string Name
    {
        get { return Consts.V_Store; }
    } 

    public GameModel GameModel
    { 
        get => m_GameModel; 
    }

    #endregion
    #region 方法

    public CardInfo RandomCard() //随机抽一张卡
    {
        CardInfo cardInfo = GameModel.Cards[UnityEngine.Random.Range(0, GameModel.CardCount)];
        return cardInfo;
    }
    #endregion

    #region Unity回调
    #endregion

    #region 事件回调
    public override void RegisterEvents()
    {
        AttentionEvents.Add(Consts.E_EnterScene); 
    }

    public override void HandleEvent(string eventName, object data = null)
    {
        switch (eventName)
        {
            case Consts.E_EnterScene:
                SceneArgs e0 = data as SceneArgs;
                if (e0.SceneIndex == 1)
                {
                    m_GameModel = GetModel<GameModel>();
                    PlayerModel pModel = GetModel<PlayerModel>();
                }
                break;
            default: break;
        }
    }
    #endregion

    #region 帮助方法
    #endregion
}
