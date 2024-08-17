using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// 该脚本用于整个战斗阶段中卡牌移动、攻击的逻辑处理
/// 
/// </summary>
class CardAction : View
{
    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    MapBattle m_Map = null;
    RoundModel rModel = null;
    PlayerModel pModel = null;
    GameModel gModel = null;

    private Queue<MonsterCard> cardQueue = new Queue<MonsterCard>(); // 存储需要移动的卡片队列
    private Queue<TileBattle> tiles = new Queue<TileBattle>(); //存储对应的卡片格子

    private List<MonsterCard> SelfCardsOnField = new List<MonsterCard>(); //存储己方场上卡牌
    private List<MonsterCard> EnemyCardsOnField = new List<MonsterCard>(); //存储敌方场上卡牌

    private CardActionArgs cardActionArgs = null; //卡牌行动参数
    Player player;

    //判断是否完成移动和攻击 用于部分技能的实现
    int m_remainingMove = 0;
    //判断先攻击后移动的情况
    bool IsAttacked = false;
    
    //协程锁 同时只能有一个卡牌进行攻击
    private bool isCardAttacking = false;
    #endregion

    #region 属性
    public override string Name
    {
        get { return Consts.V_CardMove; }
    }
    #endregion

    #region 方法

    //移动卡片动画控制
    IEnumerator MoveCard(MonsterCard card, Vector3 targetPosition) //传入被移动卡片和目标位置
    {
        //动画控制
        card.IsCardMoving = true;
        card.NextDes = targetPosition;
        yield return new WaitWhile(() => GetIsCardMoving(card));
        Debug.Log("Card moved successfully.");
    }


    //移动卡牌到目标位置
    private IEnumerator MoveCardToTile(MonsterCard card, TileBattle startTile, int targetX, int targetY)
    {
        TileBattle targetTile = m_Map.GetTile(targetX, targetY);
        if (targetTile != null && targetX != startTile.X)
        {
            Vector3 targetPosition = m_Map.GetPosition(targetTile);
            yield return StartCoroutine(MoveCard(card, targetPosition));

            //更新卡牌位置
            targetTile.Card = startTile.Card;
            startTile.Card = null;
        }
    }

    //根据剩余可移动距离移动卡片 需要进行两次调用 移动->攻击->移动
    private IEnumerator MoveCardWithRemainingDistance(MonsterCard card, TileBattle startTile, int remainingMove, Player player)
    {
        if (remainingMove == 0) yield break;

        //获取移动距离
        int moveRange = remainingMove;
        int targetX = GetTargetX(startTile.X, moveRange, player);

        //查看是否有敌方阻挡
        targetX = CheckEnemyObstruction(startTile.X, startTile.Y, targetX, player);

        //当路径上没有敌方单位阻挡后，再次检查是否有自己的单位阻挡
        targetX = CheckSelfObstruction(startTile.X, startTile.Y, targetX, player);

        //修改剩余可移动距离
        m_remainingMove = moveRange - Mathf.Abs(targetX - startTile.X);

        //移动到目标位置
        yield return StartCoroutine(MoveCardToTile(card, startTile, targetX, startTile.Y));
    }


    //卡片攻击逻辑控制 
    IEnumerator CardAttack(MonsterCard card, TileBattle startTile) //传入需要进行攻击的卡牌以及卡牌所处的格子
    {
        //找到攻击目标
        List<TileBattle> attackTargets = GetCardsBeAttacked(card, startTile);
        //一般而言，攻击目标只有一个，但是也有可能有多个，这里暂时只考虑一个攻击目标的情况
        //TODO:多个攻击目标的情况
        if (attackTargets.Count == 0)
        {
            Debug.Log("No target to attack.");
            yield break;
        }

        //当前攻击目标
        MonsterCard target;
        //攻击玩家
        if (attackTargets[0].X == 0)
        {
            target = rModel.SelfSummoner; 
        }
        //攻击敌方玩家
        else if (attackTargets[0].X == MapBattle.ColumnCount - 1)
        {
            target = rModel.EnemySummoner;
        }
        //攻击卡牌
        else
        {
            target = attackTargets[0].Card.GetComponent<MonsterCard>();
        }

        ////动画控制 废案 用移动替代动画
        //card.IsCardAttacking = true;

        ////获取攻击目标的位置和当前卡牌的位置
        //card.NextDes = m_Map.GetPosition(attackTargets[0]);
        //card.CurPos = m_Map.GetPosition(startTile);
        //yield return new WaitWhile(() => GetIsCardAttack(card)); //等待攻击动画完成

        //现案 直接使用攻击动画
        yield return StartCoroutine(card.Attack(card, target));
        Debug.Log("Card attacked successfully.");

        //攻击完成
        IsAttacked = true;
    }

    //卡片攻击逻辑控制 传入当前卡和目标卡 外部访问接口
    //需要和另外一个攻击协程同步 同时只能有一个卡牌进行攻击
    public IEnumerator CardAttack(MonsterCard attacker, MonsterCard targetCard)
    {
        // 等待锁释放
        yield return new WaitUntil(() => !isCardAttacking);

        // 获取锁
        isCardAttacking = true;

        ////动画控制
        //attacker.IsCardAttacking = true;

        ////更新目标
        //attacker.Target = targetCard;
        ////获取攻击目标的位置和当前卡牌的位置

        //attacker.NextDes = m_Map.GetPosition(GetTileUnderCard(targetCard));
        //attacker.CurPos = m_Map.GetPosition(GetTileUnderCard(attacker));
        //yield return new WaitWhile(() => GetIsCardAttack(attacker)); //等待攻击动画完成
        //Debug.Log("Card attacked successfully.");

        // 现案 直接调用方法完成攻击
        yield return StartCoroutine(attacker.Attack(attacker, targetCard));

        //攻击完成
        IsAttacked = true;

        // 释放锁
        isCardAttacking = false;
    }
                                  

    //从队列中移动卡牌
    private IEnumerator MoveNextCard()
    {

        // 等待锁释放
        yield return new WaitUntil(() => !isCardAttacking);

        // 获取锁
        isCardAttacking = true;

        if (cardQueue.Count == 0)
        {
            // 释放锁
            isCardAttacking = false;
            yield break; // 如果队列为空，则退出协程
        }

        TileBattle startTile = tiles.Dequeue(); // 从队列中取出卡牌所在的格子
        MonsterCard ActionCard = cardQueue.Dequeue(); // 从队列中取出卡片

        //获取事件所需参数
        SelfCardsOnField = GetCardsOnField(Player.Self);
        EnemyCardsOnField = GetCardsOnField(Player.Enemy);
        cardActionArgs = new CardActionArgs(ActionCard, null, SelfCardsOnField, EnemyCardsOnField);
        ActionCard.ActionStart(cardActionArgs);//触发卡牌行动开始事件

        if (ActionCard.CantAction > 0) //如果卡片不可行动 则直接跳过当前卡牌
        {
            ActionCard.ActionFinish(cardActionArgs); //但仍需触发卡片行动结束事件
            // 释放锁
            isCardAttacking = false;
            StartCoroutine(MoveNextCard());
        }        
        else if (startTile.Card == null) //各种意外情况导致卡牌死亡
        {
            Debug.LogError("卡片不存在");
            // 释放锁
            isCardAttacking = false;
            StartCoroutine(MoveNextCard()); // 移动下一个卡片
        }
        else
        {
            //初始化移动距离
            m_remainingMove = ActionCard.MoveRange;

            //首先得判断是否有守护者
            if (ActionCard.IsGuardian > 0)
            {
                //有守护者则先进行攻击判定
                yield return StartCoroutine(CardAttack(ActionCard, startTile));

                //攻击完成后再进行移动判定
                //只有当不存在攻击目标时才进行移动
                List<TileBattle> attackTargets = GetCardsBeAttacked(ActionCard, startTile);
                if (attackTargets.Count == 0)
                {
                    yield return StartCoroutine(MoveCardWithRemainingDistance(ActionCard, startTile, m_remainingMove, player));
                    //更新卡牌位置
                    startTile = GetTileUnderCard(ActionCard);
                }
                //如果还未进行攻击，可以再次进行攻击判定
                if (!IsAttacked)
                {
                    yield return StartCoroutine(CardAttack(ActionCard, startTile));
                }
            }
            //没有守护者
            else if (ActionCard.IsGuardian <= 0)
            {
                //第一次移动判断 这里的可移动距离是卡牌的移动距离属性
                yield return StartCoroutine(MoveCardWithRemainingDistance(ActionCard, startTile, m_remainingMove, player));
                //更新卡牌位置
                startTile = GetTileUnderCard(ActionCard);

                //攻击判定
                yield return StartCoroutine(CardAttack(ActionCard, startTile));

                // 第二次移动判断 这里的可移动距离是剩余可移动距离
                yield return StartCoroutine(MoveCardWithRemainingDistance(ActionCard, startTile, m_remainingMove, player));
            }

            //更新参数
            //获取事件所需参数
            SelfCardsOnField = GetCardsOnField(Player.Self);
            EnemyCardsOnField = GetCardsOnField(Player.Enemy);
            cardActionArgs = new CardActionArgs(ActionCard, null, SelfCardsOnField, EnemyCardsOnField);

            ActionCard.ActionFinish(cardActionArgs); //卡行动结束

            //重置参数
            m_remainingMove = 0;
            IsAttacked = false;

            // 释放锁
            isCardAttacking = false;

            StartCoroutine(MoveNextCard()); // 移动下一个卡片
        }

    }

    #endregion

    #region Unity回调
    #endregion

    #region 事件回调
    public override void RegisterEvents()
    {
        AttentionEvents.Add(Consts.E_EnterScene);
        AttentionEvents.Add(Consts.E_ActAUnit);
        AttentionEvents.Add(Consts.E_ActAll);
        AttentionEvents.Add(Consts.E_EndLevel);
    }

    public override void HandleEvent(string eventName, object data = null)
    {
        switch (eventName)
        {
            case Consts.E_EnterScene:
                SceneArgs e0 = data as SceneArgs;
                if (e0.SceneIndex == 3)
                {
                    //获取数据
                    gModel = GetModel<GameModel>();
                    pModel = GetModel<PlayerModel>();
                    rModel = GetModel<RoundModel>();

                    //加载地图
                    m_Map = GetComponent<MapBattle>();

                }
                break;

            case Consts.E_ActAll:
                ActAllArgs e1 = data as ActAllArgs;
                player = e1.player;
                // 获取排序后的格子列表
                List<TileBattle> sortedTiles = GetTilesInOrder(e1.player);
                if (sortedTiles == null || sortedTiles.Count == 0)
                {
                    Debug.LogWarning("No sorted tiles available.");
                    return;
                }

                //遍历格子列表，移动符合条件的卡片
                foreach (TileBattle tile in sortedTiles)
                {
                    MonsterCard card = tile.Card?.GetComponent<Card>() as MonsterCard; // 获取卡片组件
                    if (card != null && card.Player == e1.player)
                    {
                        tiles.Enqueue(tile);
                        cardQueue.Enqueue(card); // 将卡片加入队列
                    }
                }

                //移动卡片，协程的最开始入口
                StartCoroutine(MoveNextCard()); 
                break;

            case Consts.E_EndLevel:
                StopAllCoroutines();
                break;

            default: break;
        }

    }
    #endregion

    #region 帮助方法
    //对格子进行排序，返回排序后的格子列表
    //按照玩家的不同，排序规则不同，该方法用于获取需要移动的卡片的顺序
    public List<TileBattle> GetTilesInOrder(Player player)
    {
        List<TileBattle> sortedTiles = new List<TileBattle>();
        // 1. 定义排序规则
        if (player == Player.Self)
        {
            sortedTiles = m_Map.Grid.OrderByDescending(tile => tile.X)
                         .ThenByDescending(tile => tile.Y)
                         .ToList();
        }
        else if (player == Player.Enemy)
        {
            sortedTiles = m_Map.Grid.OrderBy(tile => tile.X)
                         .ThenByDescending(tile => tile.Y)
                         .ToList();
        }
        else
        {
            Debug.LogWarning("Unknown player type.");
            sortedTiles = null;
        }
        return sortedTiles;
    }

    //获取目标位置 不考虑阻挡
    //返回目标位置的X坐标
    private int GetTargetX(int startX, int moveRange, Player player)
    {
        if (player == Player.Self)
            return Mathf.Min(startX + moveRange, MapBattle.ColumnCount - 2);
        else
            return Mathf.Max(startX - moveRange, 1);
    }

    //检查敌方阻挡
    //返回目标位置的X坐标
    private int CheckEnemyObstruction(int startX, int startY, int targetX, Player player)
    {
        if (player == Player.Self)
        {
            for (int curX = startX + 1; curX <= targetX; curX++)
            {
                TileBattle curtile = m_Map.GetTile(curX, startY);
                if (curtile.Card && curtile.Card.GetComponent<Card>().player == Player.Enemy)
                {
                    return curX - 1;
                }
            }
        }
        else if (player == Player.Enemy)
        {
            for (int curX = startX - 1; curX >= targetX; curX--)
            {
                TileBattle curtile = m_Map.GetTile(curX, startY);
                if (curtile.Card && curtile.Card.GetComponent<Card>().player == Player.Self)
                {
                    return curX + 1;
                }
            }
        }
        return targetX;
    }

    //检查自己阻挡
    //返回目标位置的X坐标
    private int CheckSelfObstruction(int startX, int startY, int targetX, Player player)
    {
        while (targetX != startX)
        {
            TileBattle targetTile = m_Map.GetTile(targetX, startY); // Ensure startTile is updated in each iteration
            if (targetTile.Card)
            {
                if (player == Player.Self)
                    targetX -= 1;
                else
                    targetX += 1;
            }
            else
            {
                break;
            }
        }
        return targetX;
    }

    //获取卡牌攻击目标
    public List<TileBattle> GetCardsBeAttacked(MonsterCard card, TileBattle tile) //传入需要攻击的卡片和卡片所在的格子
    {
        //寻找卡牌目标
        List<TileBattle> attackTarget = new List<TileBattle>();
        int attackRange = card.AttackRange;
        int direction = card.Player == Player.Self ? 1 : -1;

        //当卡牌有守护者技能时 可以向后 以及上下两排 寻找攻击目标
        int[] rows = { 0, 1, -1 };
        if (card.IsGuardian > 0)
        {
            //先搜索身后 然后是上下两排 往前搜索
            for (int i = attackRange; i >= 0; i--)
            {   
                foreach (int j in rows)
                {
                    int targetX = tile.X - (i * direction);  // 向后搜索
                    int targetY = tile.Y + j;

                    if (targetX < 0 || targetX >= MapBattle.ColumnCount || targetY < 0 || targetY >= MapBattle.RowCount)
                    {
                        continue;  // 跳过越界的坐标
                    }
                    TileBattle targetTile = m_Map.GetTile(targetX, targetY);

                    //可以攻击玩家
                    if (targetX == 0 || targetX == MapBattle.ColumnCount - 1)
                    {
                        if (card.Player == Player.Self && targetX == MapBattle.ColumnCount - 1 ||
                            card.Player == Player.Enemy && targetX == 0)
                            attackTarget.Add(targetTile);
                        break;
                    }

                    //可以攻击敌方卡牌
                    if (targetTile.Card)
                    {
                        MonsterCard targetCard = targetTile.Card.GetComponent<MonsterCard>();
                        if ((card.Player == Player.Self && targetCard.Player == Player.Enemy) ||
                            (card.Player == Player.Enemy && targetCard.Player == Player.Self))
                        {
                            attackTarget.Add(targetTile);
                        }
                    }
                }
            }
        } 

        //向前搜索
        //TODO:有可能存在能攻击多行的情况
        for (int i = 1; i <= attackRange; i++)
        {
            int targetX = tile.X + (i * direction);

            if (targetX < 0 || targetX >= MapBattle.ColumnCount)
            {
                continue;  // 跳过越界的坐标
            }
            TileBattle targetTile = m_Map.GetTile(targetX, tile.Y);

            //可以攻击玩家
            if (targetX == 0 || targetX == MapBattle.ColumnCount - 1) 
            { 
                if (card.Player == Player.Self && targetX == MapBattle.ColumnCount - 1 || 
                    card.Player == Player.Enemy && targetX == 0)
                    attackTarget.Add(targetTile);
                break;
            }

            //可以攻击敌方卡牌
            if (targetTile.Card)
            {
                MonsterCard targetCard = targetTile.Card.GetComponent<MonsterCard>();
                if ((card.Player == Player.Self && targetCard.Player == Player.Enemy) ||
                    (card.Player == Player.Enemy && targetCard.Player == Player.Self))
                {
                    attackTarget.Add(targetTile);
                }
            }
        }

        return attackTarget;
    }

    //检测卡牌移动是否完成
    bool GetIsCardMoving(MonsterCard card)
    {
        return card.IsCardMoving;
    }

    //检测卡牌攻击是否完成
    bool GetIsCardAttack(MonsterCard card)
    {
        return card.IsCardAttacking;
    }

    bool GetIsAllCardMoved(Player player)
    {
        if (player == Player.Self)
        {
            foreach (GameObject go in rModel.PlayerSummonList)
            {
                if (go.GetComponent<MonsterCard>().IsCardMoving)
                    return false;
            }
        }
        return true;
    }

    //获取卡牌所处的格子
    public TileBattle GetTileUnderCard(MonsterCard card)
    {
        foreach (TileBattle tile in m_Map.Grid)
        {
            if (tile.Card == card.gameObject)
            {
                return tile;
            }
        }
        return null;
    }

    //获取场上卡牌集合
    public List<MonsterCard> GetCardsOnField(Player player)
    {
        List<MonsterCard> CardList = new List<MonsterCard>();
        foreach (TileBattle tile in m_Map.Grid)
        {
            //遍历格子列表，移动符合条件的卡片
            MonsterCard card = tile.Card?.GetComponent<Card>() as MonsterCard; // 获取卡片组件
            if (card != null && card.Player == player)
            {
                CardList.Add(card); // 将卡片加入队列
            }
            
        }
        return CardList;
    }

    


    #endregion


}

