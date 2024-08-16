﻿using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public static class Consts
{
    //关卡目录
    public static readonly string LevelDir = Application.dataPath + @"/Game/Resources/Res/Levels"; //application.datapath得到的是assets目录
    public static readonly string MapDir = Application.dataPath + @"/Game/Resources/Res/Maps";

    //卡牌目录
    //Resoruce.Load只能加载Resources目录下的资源 并且要使用相对路径
    public static readonly string CardDataDir = "Res/Datas";

    //buff effect skill目录
    public static readonly string BuffDataDir = Application.dataPath + @"/Game/Resources/Res/Datas/buffs.json";
    public static readonly string EffectDataDir = Application.dataPath + @"/Game/Resources/Res/Datas/effects.json";
    public static readonly string SkillDataDir = Application.dataPath + @"/Game/Resources/Res/Datas/skills.json";

    //存档
    public const string GameProgress = "GameProgress";

    //Model
    public const string M_GameModel = "M_GameModel";
    public const string M_PlayerModel = "M_PlayerModel";
    public const string M_RoundModel = "M_RoundModel";

    //View
    

    public const string V_Select = "V_Select";

    //Scene 2
    public const string V_Battle = "V_Battle";
    public const string V_Shop = "V_Shop";
    public const string V_Boss = "V_Boss";
    public const string V_Chance = "V_Chance";
    public const string V_Library = "V_Library";

    //Scene 1
    public const string V_Card = "V_Card";
    public const string V_Store = "V_Store";
    public const string V_OpenPackage = "V_OpenPackage";
    public const string V_Start = "V_Start";

    //Scene 4
    public const string V_Deck = "V_Deck";
    public const string V_ClickCard = "V_ClickCard";
    public const string V_Back = "V_Back";

    //Scene3
    public const string V_CardList = "V_CardList";
    public const string V_CountRound = "V_CountRound";
    public const string V_ShowRound = "V_ShowRound";
    public const string V_BattleCard = "V_BattleCard";
    public const string V_Spawner = "V_Spawner";
    public const string V_Spell = "V_Spell";    
    public const string V_UnitStatus = "V_UnitStatus";
    public const string V_ButtonBattle = "V_ButtonBattle";
    public const string V_CardMove = "V_CardMove";
    public const string V_UIInformationWindow = "V_UIInformationWindow";
    public const string V_Summoner = "V_Summoner";
    public const string V_Win = "V_Win";
    public const string V_Lost = "V_Lost";


    //Controller
    public const string E_StartUp = "E_StartUp";

    public const string E_EnterScene = "E_EnterScene"; //SceneArgs
    public const string E_ExitScene = "E_ExitScene"; //SceneArgs

    public const string E_StartLevel = "E_StartLevel"; //SceneArgs

    public const string E_ShowBattlePanel = "E_BattlePanel"; //ShowSpawnPanelArgs
    public const string E_ClickCard = "E_ClickCard"; //ClickCardArgs

    public const string E_DrawCard = "E_DrawCard"; //DrawCardArgs
    public const string E_ShowDrawCard = "E_ShowDrawCard"; 

    public const string E_StartRound = "E_StartRound"; //StartRoundArgs
    public const string E_SummonCardRequest = "E_SummonCardRequest"; //SummonCardRequestArgs
    public const string E_CancelSummon = "E_CancelSummon";
    public const string E_ConfirmSummon = "E_ConfirmSummon"; //ConfirmSummonArgs
    public const string E_TurnEnd = "E_TurnEnd";
    public const string E_AddCardToEnemyHand = "E_AddCardToEnemyHand"; //AddCardToEnemyHandArgs
    public const string E_EnemySummon = "E_EnemySummon"; //EnemySummonArgs
    public const string E_CostReduce = "E_CostReduce"; //CostReduceArgs
    public const string E_ActAUnit = "E_ActAUnit"; //ActAUnitArgs
    public const string E_ActAll = "E_ActAll"; //ActAllArgs
    public const string E_UseSpellCardRequest = "E_UseSpellCardRequest"; //UseSpellCardRequestArgs  
    public const string E_ConfirmSpell = "E_ConfirmSpell"; //ConfirmSpellArgs
    public const string E_EndLevel = "E_EndLevel"; //EndLevelArgs



    //SpellNames
    public static readonly string[] EffectNames = new string[]
    {
        "Damage1",
        "Heal1",
        "Damage2",
        "Heal2",
        "Damage3",
        "Heal3",
        "Damage4",
        "Heal4",
    };

    //BuffNames
    public static readonly string[] BuffNames = new string[]
    {
        "Brave1",
        "Bleed1",
        "Brave1FromSkill",
        "Weaken05",
        "Guardian",
        "GuardianFromSkill",
        "Stun1",
        "MoveBoost14",
        "CounterBackFromSkill",
    };

    //SkillNames
    public static readonly string[] SkillNames = new string[]
    {
        "Brave1",
        "Bleed1",
        "Weaken05",
        "Guardian",
        "Stun1",
        "WindWalk4",
        "CounterBack",
        "HealAnAlly2",
    };
}


public enum BuildingType
{
    Init,
    Chest,
    Battle,
    Shop,
    Boss,
    Exit,
    Chance,
    Path,
}

public enum Direction
{
    Top,
    Right,
    Left,
    Bottom,
}

public enum CardState
{
    Unavailable,
    Library,
    Deck
}

public enum GamePhase
{
    PlayerDraw,
    PlayerAct,
    PlayerBattle,
    EnemyDraw,
    EnemyAct,
    EnemyBattle
}

public enum CardType
{
    Monster,
    Spell,
    Summoner,
}

public enum CardStateBattle //战斗场景中卡牌状态
{
    inHand,
    inFeild,
    inGrave,
    inDeck,
}

public enum ReduceMethod
{
    All,
    Random,
}

public enum Player
{
    Self,
    Enemy,
}

//法术作用对象
public enum SpellType
{
    Self,
    Enemy,
    Any,
    FieldSelf,
    FieldEnemy,
    FieldAny,
}

//Buff触发机制
public enum BuffEvent
{
    OnRoundStart,
    OnRoundEnd,
    OnDie,
    OnAttack,
    OnTakeDamage,
    OnTakeHeal,
    OnActionFinish,
    OnMove,
}
