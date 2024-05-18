using System;
using System.Collections.Generic;
using UnityEngine;


//战斗界面 格子信息
public class TileBattle : Tile
{
    public bool CanSetMe; //是否可以放置单位
    public bool CanSetEnemy; //敌人是否可以防止单位
    public bool CanHold; //单位是否可以进入
    public GameObject Card; //格子保存的卡牌信息 

    public TileBattle(int x, int y):base(x, y)
    {
        X = x;
        Y = y;
    }
}