using System;
using System.Collections;
using UnityEngine;


//选择界面，格子信息 目前还没有使用 这里做好接口 给以后拓展留空间
//Room用于存放每个格子的建筑信息和关联格子的信息
//LevelGenerator返回的是int 列表，对应每个格子的建筑信息，若要使用Room，需要给Room绑定对应的建筑信息
public class Room : Tile
{
    public int Current;
    public int top;
    public int right;
    public int left;
    public int bottom;
    public int BuildingID; //该格子存放的单位信息

    public Room(int x, int y, int buildingID):base(x, y)
    {
        X = x; 
        Y = y;
        BuildingID = buildingID;
    }
}