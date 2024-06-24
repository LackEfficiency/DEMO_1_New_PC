using System;
using System.Collections.Generic;

public class StartUpCommand : Controller
{
    public override void Execute(object data)
    {
        //注册模型(model)
        RegisterModel(new GameModel());
        RegisterModel(new PlayerModel());
        RegisterModel(new RoundModel());

        //注册命令(controller) 
        RegisterController(Consts.E_EnterScene, typeof(EnterSceneCommand));
        RegisterController(Consts.E_ExitScene, typeof(ExitSceneCommand));
        RegisterController(Consts.E_StartLevel, typeof(StartLevelCommand));
        RegisterController(Consts.E_DrawCard, typeof(DrawCardCommand));
        RegisterController(Consts.E_TurnEnd, typeof(TurnEndCommand));
        RegisterController(Consts.E_AddCardToEnemyHand, typeof(AddCardToEnemyHandCommand));
        RegisterController(Consts.E_CostReduce, typeof(ReduceCostCommand));
        //注册视图(view)需要在进入场景后动态注入


        //初始化
        GameModel gModel = GetModel<GameModel>();
        gModel.IsPlaying = true;
        PlayerModel pModel = GetModel<PlayerModel>();

        gModel.Initialize();
        pModel.Initialize();

        //初始化playermodel所有卡牌的长度
        pModel.m_Library = new int[gModel.CardCount];
        pModel.m_Deck = new int[gModel.CardCount];

        //初始化pm,从存档里加入
        pModel.LoadFileData();

        //跳转到开始界面
        Game.Instance.LoadScene(1);

    }
}
