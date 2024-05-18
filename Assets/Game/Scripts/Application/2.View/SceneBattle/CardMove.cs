using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

class CardMove : View
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
    TileBattle target;

    private Queue<Card> cardQueue = new Queue<Card>(); // 存储需要移动的卡片队列
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
    bool GetIsCardMoving(Card card)
    {
        return card.IsCardMoving;
    }

    bool GetIsAllCardMoved(Player player)
    {
        if (player == Player.Self)
        {
            foreach (GameObject go in rModel.PlayerSummonList)
            {
                if (go.GetComponent<Card>().IsCardMoving)
                    return false;
            }
        }
        return true;
    }

    //移动卡片
    IEnumerator MoveCard(Card card, Vector3 targetPosition)
    {
        card.IsCardMoving = true;
        card.NextDes = targetPosition;
        yield return new WaitWhile(() => GetIsCardMoving(card));
        Debug.Log("Card moved successfully.");
    }

    //从队列中移动卡牌
    private IEnumerator MoveNextCard()
    {
        if (cardQueue.Count == 0)
        {
            yield break; // 如果队列为空，则退出协程
        }

        TileBattle tile = tiles.Dequeue();
        Card nextCard = cardQueue.Dequeue();

        // 计算移动目标位置 防止超出地图边界
        if (player == Player.Self)
            targetX = Mathf.Min(tile.X + 2, MapBattle.ColumnCount - 1);
        else
            targetX = Mathf.Max(tile.X - 2, 0);


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
        else
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


        //查看是否有其他阻挡
        while (targetX != tile.X)
        {
            target = m_Map.GetTile(targetX, tile.Y);
            if (target.Card) //如果有阻挡 则向后找寻一个格子
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
        if (target != null && targetX != tile.X)
        {
            Vector3 targetPosition = m_Map.GetPosition(target);
           // Debug.Log(targetPosition);
            yield return StartCoroutine(MoveCard(nextCard, targetPosition));
            //更新
            target.Card = tile.Card;
            tile.Card = null;
        }

        StartCoroutine(MoveNextCard()); // 移动下一个卡片
    }
    #endregion

    #region Unity回调
    #endregion

    #region 事件回调
    public override void RegisterEvents()
    {
        AttentionEvents.Add(Consts.E_EnterScene);
        AttentionEvents.Add(Consts.E_MoveAUnit);
        AttentionEvents.Add(Consts.E_MoveAll);
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

            case Consts.E_MoveAll:
                MoveAllArgs e1 = data as MoveAllArgs;
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
                    Card card = tile.Card?.GetComponent<Card>(); // 获取卡片组件
                    if (card != null && card.Player == e1.player)
                    {
                        tiles.Enqueue(tile);
                        cardQueue.Enqueue(card); // 将卡片加入队列
                    }
                }

                StartCoroutine(MoveNextCard()); 
                break;

            default: break;
        }

    }
    #endregion

    #region 帮助方法
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
    #endregion
}

