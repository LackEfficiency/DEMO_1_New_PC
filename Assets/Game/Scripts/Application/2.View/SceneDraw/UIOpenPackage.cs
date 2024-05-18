using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIOpenPackage : View
{
    #region 常量
    #endregion

    #region 事件
    #endregion 

    #region 字段
    public GameObject m_CardPrefab; //显示的卡片
    public GameObject m_CardPool; //用于限制卡片位置

    public Button btnOpenPackage; //开包按钮

    UIStore m_CardStore; 
    List<GameObject> m_cards = new List<GameObject> (); //存放的卡片

    PlayerModel m_PlayerModel;
    GameModel m_GameModel;
    #endregion

    #region 属性
    public override string Name
    {
        get { return Consts.V_OpenPackage; }
    }

    public UIStore CardStore 
    { 
        get => m_CardStore; 
    }

    public PlayerModel PlayerModel 
    { 
        get => m_PlayerModel; 
    }

    public GameModel GameModel 
    { 
        get => m_GameModel; 
    }
    #endregion

    #region 方法
    public void ClearPool()
    {
        foreach(GameObject card in m_cards)
        {
            Game.Instance.ObjectPool.Unspawn(card.gameObject);
        }
        m_cards.Clear();
    }
    #endregion

    #region Unity回调
    #endregion

    #region 事件回调
    public void OnClickOpen()
    {
        
        if (m_PlayerModel.Coins < 5)
        {
            return;
        }
        else
        {
            m_PlayerModel.Coins -= 5;
        }
         
        ClearPool();
        for (int i = 0; i < 5; i++)
        {
            GameObject go = Game.Instance.ObjectPool.Spawn("Card", "prefabs/Cards");
            UICard uICard = go.GetComponent<UICard>();

            uICard.CardInfo = CardStore.RandomCard(); //抽一张卡
            uICard.State = CardState.Library; //加入卡堆
            uICard.transform.parent = m_CardPool.transform; //移动卡牌位置至网格

            uICard.m_Count.gameObject.SetActive(false); //隐藏数量显示
         
            uICard.Show(); //展示抽的卡
            PlayerModel.SaveCardData(uICard.CardInfo.CardID);//储存玩家卡牌
            m_cards.Add(go); //卡牌里加入抽的卡 
        }      
    }

    public override void RegisterEvents()
    {
        AttentionEvents.Add(Consts.E_EnterScene);
        AttentionEvents.Add(Consts.E_ExitScene);
    }

    public override void HandleEvent(string eventName, object data = null)
    {
        switch (eventName)
        {
            case Consts.E_EnterScene:
                SceneArgs e0 = data as SceneArgs;
                if (e0.SceneIndex == 1)
                {
                    m_CardStore = GetComponent<UIStore>();
                    m_PlayerModel = GetModel<PlayerModel>();
                }
                break;
            default: 
                break;

        }
    }


    #endregion

    #region 帮助方法
    #endregion
}
