using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Unity.VisualScripting;
using System;

public class RoundModel : Model
{
    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    public bool m_IsCountDownComplete;
    int m_MaxRounds = 99; //最大回合数

    List<Round> m_Rounds = new List<Round>(); //储存每个回合的敌人类型
    int m_RoundIndex = -1; //当前所在回合的索引
    bool m_AllRoundsComplete = false; // 是否所有敌人都已生成

    private GamePhase m_GamePhase = GamePhase.PlayerDraw; //游戏状态
                                                        
    private List<CardInfo> m_CardsDrawn = new List<CardInfo>(); //由于deck到hand是cardinfo到gameobject的转化 需要一个过度列表

    private List<CardInfo> m_PlayerDeckList = new List<CardInfo>(); //玩家当前卡组 仅在战斗场景使用
    private List<GameObject> m_PlayerSummonList = new List<GameObject>(); //玩家场上卡牌集合
    private List<GameObject> m_PlayerGraveList = new List<GameObject>(); //玩家墓地卡牌集合
    private List<GameObject> m_PlayerHandList = new List<GameObject>(); //玩家当前手牌

    private List<CardInfo> m_EnemyHandList = new List<CardInfo>();//敌人当前卡组 每回合从回合信息里添加 敌人卡组没有手牌上限
    private List<GameObject> m_EnemyGraveList = new List<GameObject>();//敌人当前墓地
    private List<GameObject> m_EnemySummonList = new List<GameObject>(); //敌人当前场上卡组集合

    private bool m_IsRoundRun = true; //是否回合还在进行
    private bool m_IsTurnRun = true; //是否阶段还在进行 用于控制回合自动结束 

    #endregion

    #region 属性
    public override string Name
    {
        get { return Consts.M_RoundModel; }
    }

    public int RoundIndex
    {
        get => m_RoundIndex;
        set => m_RoundIndex = value;
    }

    public List<Round> Rounds
    {
        get => m_Rounds;
        set => m_Rounds = value;
    }

    public bool AllRoundsComplete
    {
        get => m_AllRoundsComplete;
    }

    public GamePhase GamePhase
    {
        get => m_GamePhase;
        set => m_GamePhase = value;
    }
    public List<CardInfo> PlayerDeckList
    {
        get => m_PlayerDeckList;
    }
    public List<GameObject> PlayerSummonList
    {
        get => m_PlayerSummonList;
    }
    public List<GameObject> PlayerGraveList
    {
        get => m_PlayerGraveList;
    }
    public List<GameObject> PlayerHandList
    {
        get => m_PlayerHandList;
    }
    public List<CardInfo> EnemyHandList
    {
        get => m_EnemyHandList;
    }
    public List<GameObject> EnemyGraveList
    {
        get => m_EnemyGraveList;
    }
    public List<GameObject> EnemySummonList
    {
        get => m_EnemySummonList;
    }
    public int MaxRounds
    {
        get => m_MaxRounds;
    }
    public bool IsRoundRun
    {
        get => m_IsRoundRun;
        set => m_IsRoundRun = value;
    }

    public bool IsTurnRun { get => m_IsTurnRun; set => m_IsTurnRun = value; }
    public List<CardInfo> CardsDrawn { get => m_CardsDrawn; set => m_CardsDrawn = value; }



    #endregion

    #region 方法
    public void GameStart()
    {
        // 读取数据
        // 卡组洗牌
        // 玩家抽卡，敌人抽卡
    }

    public void TurnEnd() //阶段结束自动调用
    {
        IsTurnRun = true;
        //切换阶段
        if (GamePhase == GamePhase.PlayerDraw)
        {
            GamePhase = GamePhase.PlayerAct;
        }
        else if (GamePhase == GamePhase.PlayerAct)
        {
            GamePhase = GamePhase.PlayerBattle;
        }
        else if (GamePhase == GamePhase.PlayerBattle)
        {
            GamePhase = GamePhase.EnemyDraw;
        }
        else if (GamePhase == GamePhase.EnemyDraw)
        {
            GamePhase = GamePhase.EnemyAct;  
        }
        else if (GamePhase == GamePhase.EnemyAct)
        {
            GamePhase = GamePhase.EnemyBattle;
        }
        else if (GamePhase == GamePhase.EnemyBattle)
        {
            GamePhase = GamePhase.PlayerDraw;
            IsRoundRun = false;
        }
        //Debug.Log("当前阶段" + GamePhase.ToString());
    }

    //每次进入关卡时调用，读取玩家卡组数据
    public void LoadPlayerDeck(int[] deck, GameModel gm)
    {
        //将[]数据存储为List，方便游戏中调用
        for (int i = 0; i < deck.Length; i++)
        {
            if (deck[i] != 0)
            {
                int count = deck[i];
                for (int j = 0; j < count; j++)
                {
                    CardInfo card = gm.CopyCard(i);
                    PlayerDeckList.Add(card);
                }
            }
        }
    }

    // 读取敌人卡组数据 每回合调用
    public void LoadEnemyHand()
    {
        if (Rounds == null || Rounds.Count <= RoundIndex || RoundIndex < 0)
        {
            Debug.LogError("LoadEnemyHand: Invalid Rounds or RoundIndex.");
            return;
        }

        List<int> enemyIDs = Rounds[RoundIndex].EnemyID;
        if (enemyIDs == null || enemyIDs.Count == 0)
        {
            Debug.LogWarning("LoadEnemyHand: EnemyID array is null or empty.");
            return;
        }

        AddCardToEnemyHandArgs e = new AddCardToEnemyHandArgs();
        e.cardsID = enemyIDs;
        SendEvent(Consts.E_AddCardToEnemyHand, e);
    }
    #endregion

    //洗牌
    public void ShuffleDeck()
    {
        Tools.ShuffleList(PlayerDeckList);
    }

    //抽卡
    public void DrawCard(int count)
    {
        if (count <= 0)
        {
            return; // 不需要抽取卡牌，直接返回
        }

        // 检查是否需要移除的卡牌数超过了牌组数量
        int cardsToRemove = Mathf.Min(count, PlayerDeckList.Count);

        // 将需要移除的卡牌添加到临时手牌列表中
        CardsDrawn = PlayerDeckList.GetRange(0, cardsToRemove);

        // 从卡组中移除已抽取的卡牌
        PlayerDeckList.RemoveRange(0, cardsToRemove);
    }

    public void StartRound()
    {
        Game.Instance.StartCoroutine(RunRound());
    }

    public void StopRound()
    {
        Game.Instance.StopCoroutine(RunRound());
    }

    //回合控制
    IEnumerator RunRound()
    {
        for (int i = 0; i < MaxRounds; i++)
        {
            IsRoundRun = true;
            //回合开始 开始回合倒计时
            m_IsCountDownComplete = false;
            StartRoundArgs e = new StartRoundArgs();
            e.RoundIndex = i;

            SendEvent(Consts.E_StartRound, e);  
            yield return new WaitUntil(() => GetIsCountDownComplete());

            //回合数
            RoundIndex = i;

            //小阶段循环
            bool isRoundRunning = true;
            while (isRoundRunning)
            {
                if (GamePhase == GamePhase.PlayerDraw)
                {
                    // 玩家抽卡阶段逻辑
                    yield return Game.Instance.StartCoroutine(PlayerDrawPhase());
                    TurnEnd();
                }
                else if (GamePhase == GamePhase.PlayerAct)
                {
                    // 玩家行动阶段逻辑
                    yield return Game.Instance.StartCoroutine(PlayerActPhase());
                    TurnEnd();
                }
                else if (GamePhase == GamePhase.PlayerBattle)
                {
                    // 玩家战斗阶段逻辑
                    yield return Game.Instance.StartCoroutine(PlayerBattlePhase());
                    TurnEnd();
                }
                else if(GamePhase == GamePhase.EnemyDraw)
                {
                    //敌人抽卡阶段
                    yield return Game.Instance.StartCoroutine(EnemyDrawPhase());
                    TurnEnd();
                }
                else if(GamePhase == GamePhase.EnemyAct)
                {
                    //敌人行动阶段
                    yield return Game.Instance.StartCoroutine(EnemyActPhase());
                    TurnEnd();
                }
                else if(GamePhase == GamePhase.EnemyBattle)
                {
                    //敌人战斗阶段
                    yield return Game.Instance.StartCoroutine(EnemyBattlePhase());
                    TurnEnd();
                }
                // 判断回合是否结束，如果结束则退出循环
                isRoundRunning = GetIsRoundRun();
            }
        }
    }
    IEnumerator PlayerDrawPhase()
    {
        // 玩家抽卡阶段逻辑
        // ...
        //第一步抽卡
        DrawCardArgs e = new DrawCardArgs();
        if (RoundIndex == 0)  //第一回合就抽3张
            e.nums = 3;
        else
            e.nums = 1;
        SendEvent(Consts.E_DrawCard, e); //发送抽卡事件
        SendEvent(Consts.E_ShowDrawCard, e); //发送展示事件

        //第二步所有卡牌cost减一
        CostReduceArgs e0 = new CostReduceArgs();
        e0.reduceMethod = ReduceMethod.All;
        e0.player = Player.Self;
        SendEvent(Consts.E_CostReduce, e0);

        IsTurnRun = false; //自动回合结束

        // 等待条件满足
        yield return new WaitWhile(() => IsPlayerDrawPhaseComplete());
    }

    IEnumerator PlayerActPhase()
    {
        // 玩家行动阶段逻辑
        // ...
       
        // 等待条件满足
        yield return new WaitWhile(() => IsPlayerActPhaseComplete());
    }

    IEnumerator PlayerBattlePhase()
    {
        // 玩家战斗阶段逻辑
        // ...
        //第一步 移动所有的卡牌
        //每张卡牌移动完都需要进行战斗阶段的判定
        ActAllArgs e = new ActAllArgs();
        e.player = Player.Self;
        SendEvent(Consts.E_ActAll, e);

        // 等待条件满足
        yield return new WaitWhile(() => IsPlayerBattlePhaseComplete());
    }

    IEnumerator EnemyDrawPhase()
    {
        // 敌人抽卡阶段逻辑
        // ...

        //第一步抽卡
        LoadEnemyHand();
/*        foreach(CardInfo cardInfo in EnemyHandList)
        {
            Debug.Log(cardInfo.CardName);
        }*/

        //第二步，所有卡牌cost减一
        CostReduceArgs e0 = new CostReduceArgs();
        e0.reduceMethod = ReduceMethod.All;
        e0.player = Player.Enemy;
        SendEvent(Consts.E_CostReduce, e0);

        IsTurnRun = false; //回合自动结束

        // 等待条件满足
        yield return new WaitWhile(() => IsEnemyDrawPhaseComplete());
    }
    IEnumerator EnemyActPhase()
    {
        // 敌人行动阶段逻辑
        // ...
        List<CardInfo> cardsToRemove = new List<CardInfo>(); // 创建临时集合用于存储需要移除的元素
        foreach (CardInfo cardinfo in EnemyHandList)
        {
            if (cardinfo.Cost == 0)
            {
                cardsToRemove.Add(cardinfo);
            }
        }
        foreach (CardInfo cardInfo in cardsToRemove)
        {
            EnemySummonArgs e1 = new EnemySummonArgs();
            e1.cardInfo = cardInfo;
            SendEvent(Consts.E_EnemySummon, e1);
        }

        // 在循环外进行移除操作
        foreach (CardInfo cardToRemove in cardsToRemove)
        {
            EnemyHandList.Remove(cardToRemove);
        }

        IsTurnRun = false; //回合自动结束

        // 等待条件满足
        yield return new WaitWhile(() => IsEnemyActPhaseComplete());
    }

    IEnumerator EnemyBattlePhase()
    {
        // 敌人战斗阶段逻辑
        // ...
        // 玩家战斗阶段逻辑
        // ...
        //第一步 移动所有的卡牌
        //每张卡牌移动完都需要进行战斗阶段的判定
        ActAllArgs e = new ActAllArgs();
        e.player = Player.Enemy;
        SendEvent(Consts.E_ActAll, e);

        // 等待条件满足
        yield return new WaitWhile(() => IsEnemyBattlePhaseComplete());
    }


    bool IsPlayerDrawPhaseComplete()
    { 
     // 判断玩家抽卡阶段是否完成的条件
        return IsTurnRun;
    }

    bool IsPlayerActPhaseComplete()
    {
        // 判断玩家行动阶段是否完成的条件
        return IsTurnRun;
    }

    bool IsPlayerBattlePhaseComplete()
    {
        // 判断玩家战斗阶段是否完成的条件
        return IsTurnRun;
    }

    bool IsEnemyDrawPhaseComplete()
    {
        //判断敌人抽卡阶段是否完成
        return IsTurnRun;
    }

    bool IsEnemyActPhaseComplete()
    {
        //判断敌人行动阶段是否完成
        return IsTurnRun;
    }

    bool IsEnemyBattlePhaseComplete()
    {
        //判断敌人行动阶段是否完成
        return IsTurnRun;
    }

    //检测是否回合结束
    bool GetIsRoundRun()
    {
        return IsRoundRun;
    }

    #region Unity回调
    #endregion

    #region 事件回调
    bool GetIsCountDownComplete()
    {
        return m_IsCountDownComplete;
    }
    #endregion

    #region 帮助方法
    #endregion
}