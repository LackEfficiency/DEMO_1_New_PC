using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

class MoveAUnitCommand : Controller
{
    MapBattle m_Map;
    List<TileBattle> sortedTiles;

    public override void Execute(object data = null)
    {
        // 获取 MapBattle 组件
        if (m_Map == null)
        {
            m_Map = GetComponent<MapBattle>();
            if (m_Map == null)
            {
                Debug.LogError("MapBattle component not found.");
                return;
            }
        }

        MoveAUnitArgs e =  data as MoveAUnitArgs;

        // 获取排序后的格子列表
        GetTilesInOrder(e.player);
        if (sortedTiles == null || sortedTiles.Count == 0)
        {
            Debug.LogWarning("No sorted tiles available.");
            return;
        }

        // 遍历格子列表，移动符合条件的卡片
        foreach (TileBattle tile in sortedTiles)
        {
            Card card = tile.Card?.GetComponent<Card>(); // 获取卡片组件
            if (card != null && card.Player == e.player)
            {
                // 计算移动目标位置
                int targetX = Mathf.Min(tile.X + 2, MapBattle.ColumnCount - 1);
                TileBattle target = m_Map.GetTile(targetX, tile.Y);

                // 移动卡片到目标位置
                if (target != null)
                {
                    Vector3 targetPosition = m_Map.GetPosition(target);
                    card.Move(targetPosition);
                }
            }
        }
    }

    public void GetTilesInOrder(Player player)
    {   
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
    }
}

