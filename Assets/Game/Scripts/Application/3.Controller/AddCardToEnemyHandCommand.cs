using System;
using System.Collections.Generic;

class AddCardToEnemyHandCommand : Controller
{
    public override void Execute(object data = null)
    {
        AddCardToEnemyHandArgs e = data as AddCardToEnemyHandArgs;
        GameModel gModel = GetModel<GameModel>();
        RoundModel rModel = GetModel<RoundModel>();

        foreach(int id in e.cardsID)
        {
            CardInfo cardInfo = gModel.CopyCard(id);
            rModel.EnemyHandList.Add(cardInfo);
        }
    }
}

