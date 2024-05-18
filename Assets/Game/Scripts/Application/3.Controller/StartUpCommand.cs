﻿using System;
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
        PlayerModel playerModel = GetModel<PlayerModel>();
        gModel.Initialize();
        playerModel.Initialize();

        //跳转到开始界面
        Game.Instance.LoadScene(1);

    }
}
