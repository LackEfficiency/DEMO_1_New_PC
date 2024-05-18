using System;
using System.Collections.Generic;

class LevelGenerator
{
    #region 常量
    //控制一个Floor下最小房间和最大房间数
    const int MINLEVELS = 15;
    const int MAXLEVELS = 20;
    #endregion

    #region 事件
    #endregion

    #region 字段
    //地图上总计格子数，用来对应MapSelect
    private int m_gridCounts;

    //绘制边界，防止设置图标在边界上，目前还没有使用，暂时放在一边
    List<int> m_borders = new List<int>();

    //类需要返回的列表，包含每个对应位置的建筑索引，默认为空，-1
    List<int> m_rooms = new List<int>();

    //当前Floor最终房间数
    int RoomNums;

    //起始房间索引
    int IndexCenter;

    //Boss房间索引
    int IndexBoss;
    #endregion

    #region 属性
    public List<int> Borders
    {
        get => m_borders;
    }

    public int GridCounts
    {
        get => m_gridCounts;
        set => m_gridCounts = value;
    }


    #endregion

    #region 方法

    //填充边界，暂时未使用
    public void FillBorders()
    {
        //设置边界index
        for (int i = 0; i < MapSelect.RowCount; i++)
            Borders.Add(i);
        for (int i = GridCounts - MapSelect.RowCount; i < GridCounts; i++)
            Borders.Add(i);
        for (int i = 1; i < GridCounts / MapSelect.RowCount - 1; i++)
            Borders.Add(i * MapSelect.RowCount);
        for (int i = 1; i < GridCounts / MapSelect.RowCount - 1; i++)
            Borders.Add(i * MapSelect.RowCount + MapSelect.RowCount - 1);
    }

    //链接房间，暂时未使用
    public int ConnectRoom(int current, int direction)
    {
        int to = -1; //目标房间索引
        //设置四周房间
        switch (direction)
        {
            case 0:
                if (Borders.Exists(t => t == current + GridCounts / MapSelect.RowCount))
                    to = -1;
                else
                    to = current + GridCounts / MapSelect.RowCount;
                break;
            case 1:
                if (Borders.Exists(t => t == current + 1))
                    to = -1;
                else
                    to = current + 1;
                break;
            case 2:
                if (Borders.Exists(t => t == current - 1))
                    to = -1;
                else
                    to = current - 1;
                break;
            case 3:
                if (Borders.Exists(t => t == current - GridCounts / MapSelect.RowCount))
                    to = -1;
                else
                    to = current - GridCounts / MapSelect.RowCount;
                break;
        }
        return to;
    }

    //填充当前层的房间
    public List<int> FillRoom()
    {
        //房间总数量
        RoomNums = UnityEngine.Random.Range(MINLEVELS, MAXLEVELS);

        //先填充所有房间的索引
        for (int i = 0; i < GridCounts; i++)
        {
            m_rooms.Add(-1);
        }

        //中心索引
        IndexCenter = GridCounts / 2;
        m_rooms[IndexCenter] = (int)BuildingType.Init;

        //随机生成Boss房位置，必须处于地图外沿
        int bossX = GetOutlier();
        int bossY = GetOutlier();
        m_rooms[bossX * MapSelect.RowCount + bossY] = (int)BuildingType.Boss;

        //获取起始点索引
        int initX = MapSelect.RowCount / 2;
        int initY = (m_gridCounts / MapSelect.RowCount) / 2;


        //得到起点终点矩阵角坐标
        for (int i = 0; i < 4; i++)
        {
            int battleX = GetInner(initX, bossX);
            int battleY = GetInner(initY, bossY);
            m_rooms[battleX * MapSelect.RowCount + battleY] = (int)BuildingType.Battle;
        }

        return m_rooms;
        
    }
    #endregion

    #region Unity回调
    #endregion

    #region 事件回调
    #endregion

    #region 帮助方法
    private int GetOutlier() //得到boss房间的行和列
    {
        int i = -1;
        System.Random random = new System.Random();
        int rangeSelector = random.Next(0, 2);
        if (rangeSelector == 0)
            i = random.Next(1, 4);
        else
            i = random.Next(16, 18);
        return i;
    }

    private int GetInner(int x1, int x2) //得到两个坐标间的随机坐标
    {
        System.Random random = new System.Random();
        if (x1 > x2)
            return random.Next(x2, x1);
        else
            return random.Next(x1, x2);
    }
    #endregion
}