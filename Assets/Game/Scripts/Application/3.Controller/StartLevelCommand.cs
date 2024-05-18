using System;
using System.Collections.Generic;

class StartLevelCommand : Controller
{
    public override void Execute(object data = null)
    {
        StartLevelArgs e = data as StartLevelArgs;

        //第一步
        GameModel gModel = GetModel<GameModel>();
        gModel.StartLevel(e.LevelID);

        //第二步
        PlayerModel pModel = GetModel<PlayerModel>();

        //第二步
        RoundModel rModel = GetModel<RoundModel>();
        rModel.LoadPlayerDeck(pModel.Deck, gModel);
        rModel.ShuffleDeck();

        rModel.Rounds = gModel.PlayLevel.Rounds;
       
        //进入关卡
        Game.Instance.LoadScene(3);
    }
}