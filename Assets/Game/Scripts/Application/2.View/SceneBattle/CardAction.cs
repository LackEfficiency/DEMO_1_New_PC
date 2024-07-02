using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
    int targetX;
    TileBattle targetTile;

    private Queue<MonsterCard> cardQueue = new Queue<MonsterCard>(); // 存储需要移动的卡片队列
    private Queue<TileBattle> tiles = new Queue<TileBattle>(); //存储对应的卡片格子
    Player player;
    #endregion

    #region 属性
    public override string Name
    {
        get { return Consts.V_CardMove; }
    }
    #endregion

    #region 方法
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

    //移动卡片逻辑控制
    IEnumerator MoveCard(MonsterCard card, Vector3 targetPosition) //传入被移动卡片和目标位置
    {
        //动画控制
        card.IsCardMoving = true;
        card.NextDes = targetPosition;
        yield return new WaitWhile(() => GetIsCardMoving(card));
        Debug.Log("Card moved successfully.");
    }
    
    //卡片攻击逻辑控制
    IEnumerator CardAttack(MonsterCard card, TileBattle targetTile) //传入需要进行攻击的卡牌以及卡牌所处的格子
    {
        //找到攻击目标
        List<TileBattle> attackTargets = GetCardsBeAttacked(card, targetTile);
        //一般而言，攻击目标只有一个，但是也有可能有多个，这里暂时只考虑一个攻击目标的情况
        //TODO:多个攻击目标的情况
        if (attackTargets.Count == 0)
        {
            Debug.Log("No target to attack.");
            yield break;
        }

        //攻击玩家
        if (attackTargets[0].X == 0)
        {
            card.Target = rModel.SelfSummoner; 
        }
        //攻击敌方玩家
        else if (attackTargets[0].X == MapBattle.ColumnCount - 1)
        {
            card.Target = rModel.EnemySummoner;
        }
        //攻击卡牌
        else
        {
            card.Target = attackTargets[0].Card.GetComponent<MonsterCard>();
        }

        //动画控制
        card.IsCardAttacking = true;

        //获取攻击目标的位置和当前卡牌的位置
        card.NextDes = m_Map.GetPosition(attackTargets[0]);
        card.CurPos = m_Map.GetPosition(targetTile);
        yield return new WaitWhile(() => GetIsCardAttack(card)); //等待攻击动画完成
        Debug.Log("Card attacked successfully.");
    } 
                                  

    //从队列中移动卡牌
    private IEnumerator MoveNextCard()
    {
        if (cardQueue.Count == 0)
        {
            yield break; // 如果队列为空，则退出协程
        }

        TileBattle tile = tiles.Dequeue(); // 从队列中取出卡牌所在的格子
        MonsterCard targetCard = cardQueue.Dequeue(); // 从队列中取出卡片

        //获取移动距离
        int moveRange = targetCard.MoveRange;

        // 计算移动目标最远位置 防止超出地图边界
        if (player == Player.Self)
            targetX = Mathf.Min(tile.X + moveRange, MapBattle.ColumnCount - 2);
        else
            targetX = Mathf.Max(tile.X - moveRange, 1);


        //查看是否有敌方阻挡 如果没有阻挡 则完成循环不改变targetX的值
        if (player == Player.Self)
        {
            int curX = tile.X + 1;
            while (curX <= targetX)
            {
                TileBattle curtile = m_Map.GetTile(curX, tile.Y);
                if (curtile.Card)
                {   //找到第一个阻挡对象就立即终止循环
                    if (curtile.Card.GetComponent<Card>().player == Player.Enemy)
                    {
                        //记录当前中止的位置后一位
                        targetX = curX - 1;
                        break;
                    }
                }
                curX += 1;
            }
        }
        else if (player == Player.Enemy)
        {
            int curX = tile.X - 1;
            while (curX >= targetX)
            {
                TileBattle curtile = m_Map.GetTile(curX, tile.Y);
                if (curtile.Card)
                {   //找到第一个阻挡对象就立即终止循环
                    if (curtile.Card.GetComponent<Card>().player == Player.Self)
                    {
                        //记录当前中止的位置后一位
                        targetX = curX + 1;
                        break;
                    }
                }
                curX -= 1;
            }
        }

        //当路径上没有敌方单位阻挡后，再次检查是否有自己的单位阻挡
        //查看是否有其他阻挡
        while (targetX != tile.X)
        {
            targetTile = m_Map.GetTile(targetX, tile.Y);
            if (targetTile.Card) //如果有阻挡 则向后找寻一个格子
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
        
        // 移动卡片到目标位置
        if (targetTile != null && targetX != tile.X)
        {
            Vector3 targetPosition = m_Map.GetPosition(targetTile);
           // Debug.Log(targetPosition);
            yield return StartCoroutine(MoveCard(targetCard, targetPosition));
            //更新
            targetTile.Card = tile.Card;
            tile.Card = null;

            //进行了移动，卡牌处于新位置
            //进入战斗
            yield return StartCoroutine(CardAttack(targetCard, targetTile));
        }
        else if (targetTile != null && targetX == tile.X) 
        {
            //没有进行移动，卡牌处于原位置
            //进入战斗
            yield return StartCoroutine(CardAttack(targetCard, tile));
        }

        targetCard.ActionFinish(); //卡行动结束

        StartCoroutine(MoveNextCard()); // 移动下一个卡片
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

    //获取卡牌攻击目标
    public List<TileBattle> GetCardsBeAttacked(MonsterCard card, TileBattle tile) //传入需要攻击的卡片和卡片所在的格子
    {
        //寻找卡牌目标
        List<TileBattle> attackTarget = new List<TileBattle>();
        int attackRange = card.AttackRange;
        int direction = card.Player == Player.Self ? 1 : -1;

        for (int i = 1; i <= attackRange; i++)
        {
            int targetX = tile.X + (i * direction);
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
    #endregion


}

