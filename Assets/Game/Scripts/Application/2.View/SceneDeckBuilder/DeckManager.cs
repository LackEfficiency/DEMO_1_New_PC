using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckManager : View
{
    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    public Transform m_DeckPanel; //卡池槽
    public Transform m_LibraryPanel; //卡组槽

    PlayerModel m_PlayerModel; //玩家数据获取卡池槽和卡组槽
    GameModel m_GameModel; //所有卡牌信息

    Dictionary<int, GameObject> m_LibraryDic = new Dictionary<int, GameObject>(); //储存卡牌对象
    Dictionary<int, GameObject> m_DeckDic = new Dictionary<int, GameObject>();
    #endregion

    #region 属性
    public override string Name
    {
        get { return Consts.V_Deck; }
    }

    public PlayerModel PlayerModel 
    {
        get { return m_PlayerModel; }        
    }

    public Dictionary<int, GameObject> LibraryDic
    {
        get => m_LibraryDic;
    }
    public Dictionary<int, GameObject> DeckDic
    {
        get => m_DeckDic;
    }


    #endregion

    #region 方法
    //加载卡堆
    void LoadLibrary()
    {
        for(int i = 0; i < PlayerModel.Library.Length; i++)
        {
            if (PlayerModel.Library[i] > 0)
            {
                SpawnCard(i, true);
            }
        }
    }

    //加载卡组
    void LoadDeck()
    {
        for (int i = 0; i < PlayerModel.Deck.Length; i++)
        {
            if (PlayerModel.Deck[i] > 0)
            {
                SpawnCard(i, false);
            }
        }   
    }

    //更新卡组或卡堆
    public void UpdateCard(Card card) 
    {
        UICard uICard = card.GetComponent<UICard>();
        int id = uICard.CardInfo.CardID;

        if(uICard.State == CardState.Deck)
        {   
            PlayerModel.m_Deck[id]--;
            PlayerModel.m_Library[id]++;

            if (LibraryDic.ContainsKey(id))
            {
                GameObject goCorrespond = LibraryDic[id];
                Card cardCorrespond = goCorrespond.GetComponent<Card>();
                cardCorrespond.Counter = 1;
                goCorrespond.GetComponent<UICard>().m_Count.text = PlayerModel.m_Library[id].ToString();
            }
            else
            {
                SpawnCard(id, true);
            }

            //如果当前卡为最后一张，移动后回收对象
            card.Counter = -1;
            if (PlayerModel.m_Deck[id] == 0)
                DeckDic.Remove(id);
            else
                uICard.m_Count.text = PlayerModel.m_Deck[id].ToString();



        }

        else if(uICard.State == CardState.Library)
        {
            //如果卡组数量满三，就不能再添加了
            if (PlayerModel.m_Deck[id] == 3)
                return;

            PlayerModel.m_Deck[id]++;
            PlayerModel.m_Library[id]--;

            if (DeckDic.ContainsKey(id))
            {
                GameObject goCorrespond = DeckDic[id];
                Card cardCorrespond = goCorrespond.GetComponent<Card>();
                cardCorrespond.Counter = 1;
                goCorrespond.GetComponent<UICard>().m_Count.text = PlayerModel.m_Deck[id].ToString();
            }
            else
            {
                SpawnCard(id, false);
            }

            //如果当前卡为最后一张，移动后回收对象
            card.Counter = -1;
            if (PlayerModel.m_Library[id] == 0)
                LibraryDic.Remove(id);
            else
                uICard.m_Count.text = PlayerModel.m_Library[id].ToString();

        }
    }

    public void SpawnCard(int id, bool isLibrary)
    {
        GameObject go = Game.Instance.ObjectPool.Spawn("DeckCard", "prefabs/Cards");
        Card m_card = go.GetComponent<Card>();
        m_card.CardPosistionChange += CardAllMoved;

        UICard uICard = go.GetComponent<UICard>();
        if (isLibrary)
        {
            uICard.m_Count.text = PlayerModel.Library[id].ToString();
            m_card.Counter = PlayerModel.Library[id];
            uICard.transform.parent = m_LibraryPanel.transform; //移动卡牌位置至网格
        }
        else 
        { 
            uICard.m_Count.text = PlayerModel.Deck[id].ToString();
            m_card.Counter = PlayerModel.Deck[id];
            uICard.transform.parent = m_DeckPanel.transform; //移动卡牌位置至网格
        }

        uICard.CardInfo = m_GameModel.Cards[id];
        if (isLibrary)
        {
            uICard.State = CardState.Library;
            LibraryDic.Add(id, go); //储存对象
        }
        else
        {
            uICard.State = CardState.Deck;
            DeckDic.Add(id, go); //储存对象
        }

        uICard.Show();

    }

    void CardAllMoved(Card card)
    {
        //回收卡牌
        Game.Instance.ObjectPool.Unspawn(card.gameObject);
    }
    #endregion

    #region Unity回调
    public override void RegisterEvents()
    {
        AttentionEvents.Add(Consts.E_EnterScene);
        AttentionEvents.Add(Consts.E_ClickCard);
        AttentionEvents.Add(Consts.E_ExitScene);
    }
    #endregion

    #region 事件回调
    public override void HandleEvent(string eventName, object data = null)
    {
        switch (eventName)
        {
            case Consts.E_EnterScene:
                SceneArgs e0 = data as SceneArgs;
                if (e0.SceneIndex == 4)
                {
                    m_GameModel = GetModel<GameModel>();
                    m_PlayerModel = GetModel<PlayerModel>();

                    LoadLibrary();
                    LoadDeck();             
                }
                break;

            case Consts.E_ClickCard:  //点击卡牌事件
                ClickCardArgs e1 = data as ClickCardArgs;
                UpdateCard(e1.ClickCard);
                break;

            case Consts.E_ExitScene: //离开场景时保存卡组数据
                //保存玩家信息
                SceneArgs e2 = data as SceneArgs;
                if (e2.SceneIndex == 4)
                    PlayerModel.SaveDataToFile();
                break;

            default:
                break;
        }
    }
    #endregion

    #region 帮助方法
    #endregion
}
