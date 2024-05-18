using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class UICardList : View //用于手牌的显示
{
    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    Card m_card;
    public Transform m_Deck;
    RoundModel m_RoundModel;
    #endregion

    #region 属性
    public override string Name
    {
        get { return Consts.V_CardList; }
    }
    #endregion

    #region 方法
    public void MoveFromHand(Card card)
    {
        //回收手牌对象
        Game.Instance.ObjectPool.Unspawn(card.gameObject);
    }

    public void AddCardToHand(CardInfo cardInfo)
    {
        GameObject go = Game.Instance.ObjectPool.Spawn("BattleCard", "prefabs/Cards");
        //卡牌销毁事件
        m_card = go.GetComponent<Card>();
        m_card.CardPosistionChange += MoveFromHand;

        //卡牌初始化
        UICard uICard = go.GetComponent<UICard>();
        uICard.CardInfo = cardInfo;
        uICard.transform.parent = m_Deck.transform;
        uICard.Show();
        UIBattleCard uIBattleCard = go.GetComponent<UIBattleCard>(); //更新卡牌状态
        uIBattleCard.m_State = CardStateBattle.inHand;

        //卡牌加入手牌
        m_RoundModel.PlayerHandList.Add(go);
    }
    #endregion

    #region Unity回调
    public override void RegisterEvents()
    {
        AttentionEvents.Add(Consts.E_ShowDrawCard); 
    }
    #endregion

    #region 事件回调
    public override void HandleEvent(string eventName, object data = null)
    {
        switch (eventName)
        {
            case Consts.E_ShowDrawCard:
                {
                    DrawCardArgs e = data as DrawCardArgs;
                    m_RoundModel = GetModel<RoundModel>();
                    foreach (CardInfo cardInfo in m_RoundModel.CardsDrawn)
                    {
                        AddCardToHand(cardInfo);
                    }
                } 
                break;

            default:
                break;
        }
    }
}
#endregion

#region 帮助方法
#endregion
