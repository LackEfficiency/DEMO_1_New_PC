using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIBattleCard : View, IPointerDownHandler
{
    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    public CardStateBattle m_State = CardStateBattle.inHand;
    #endregion

    #region 属性
    public override string Name
    {
        get { return Consts.V_BattleCard; }
    }

    #endregion

    #region 方法
    #endregion

    #region Unity回调
    #endregion

    #region 事件回调
    public override void HandleEvent(string eventName, object data = null)
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //点击卡牌时发出召唤卡牌请求 
        UICard uIcard = this.GetComponent<UICard>();
        CardInfo cardInfo = uIcard.CardInfo;
        if (cardInfo.Cost == 0) //当且仅当卡牌剩余等待回合为0， 才能发出事件
        {   
            if (cardInfo.m_CardType == CardType.Monster && m_State == CardStateBattle.inHand) //怪兽卡才能发送召唤事件 
            {
                SummonCardRequestArgs e = new SummonCardRequestArgs();
                e.cardInfo = cardInfo;
                e.go = this.gameObject;
                e.player = Player.Self;
                SendEvent(Consts.E_SummonCardRequest, e);
            }
            //TODO:  魔法卡的使用

        }
        //Debug.Log("手牌点击:" + this.GetComponent<UICard>().CardInfo.CardName);
    }
    #endregion

    #region 帮助方法
    #endregion




}
