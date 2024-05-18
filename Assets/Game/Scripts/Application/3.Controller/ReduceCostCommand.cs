using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

class ReduceCostCommand : Controller
{
    List<CardInfo> cards = new List<CardInfo>();
    RoundModel rModel;
    GameModel gModel;
    public override void Execute(object data = null)
    {
        CostReduceArgs e = data as CostReduceArgs;
        gModel = GetModel<GameModel>();
        rModel = GetModel<RoundModel>();
        //获取卡组
        if (e.player == Player.Self)
        {
            foreach(GameObject card in rModel.PlayerHandList)
            {
                cards.Add(card.GetComponent<UICard>().CardInfo);
            }
        }
        else if (e.player == Player.Enemy) 
        {
            cards = rModel.EnemyHandList;
        }

        //减少cost
        if (e.reduceMethod == ReduceMethod.All)
        {
            foreach(CardInfo cardInfo in cards)
            {
                ReduceByAmount(cardInfo, e.reduceAmount);
            }
        }
        else if (e.reduceMethod == ReduceMethod.Random)
        {
            //随机选取卡牌
            List<int> shuffleCards = Tools.GenerateSequence(cards.Count);
            Tools.ShuffleList(shuffleCards);
            ReduceByAmount(cards[shuffleCards[0]], e.reduceAmount);
        }

    }

    void ReduceByAmount(CardInfo cardInfo, int amount)
    {
        //当前Cost若为0，则变为实际Cost的一般
        if (cardInfo.Cost == 0)
        {
            cardInfo.Cost = (gModel.Cards[cardInfo.CardID].Cost + 1) / 2 ;
        }
        else
        {
            cardInfo.Cost = Math.Max(0, cardInfo.Cost - amount);         
        }
    }
}

