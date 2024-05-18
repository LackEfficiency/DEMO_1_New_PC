using System.Collections;
using UnityEngine;

public class Tile
{
    public int X;
    public int Y;
    public object Data; //格子所保存的数据 目前用于存储游戏对象 例如图片

    public Tile(int x, int y)
    {
        X = x;
        Y = y;
    }
}