using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

//TODO: 1.随机生成level Class LevelGenerator

//用于描述关卡选择地图的状态
public class MapSelect : MonoBehaviour
{
    #region 常量
    public const int RowCount = 19; //行数
    
    #endregion

    #region 事件
    public event EventHandler<TileSelectClickEventArgs> OnTileClick;
    #endregion

    #region 字段
    public int ColumnCount; //列数

    float MapWidth; //地图宽
    float MapHeight; //地图高

    float TileWidth; //每一个格子的宽
    float TileHeight; //每一个格子的高

    public bool DrawGizoms = true; //是否显示网格

    List<Room> m_grid = new List<Room>(); //格子集合 

    Floor m_Floor; //当前层数数据

    //当前存档储存的地图信息
    List<int> BuildingIndex = new List<int>();
    #endregion

    #region 属性
    public Floor Floor 
    { 
        get => m_Floor;
    }

    public List<Room> Grid
    {
        get { return m_grid; }
    }

    #endregion

    #region 方法
    //填充地图建筑
    //TODO: 每次进入后场景后需要调用，若当前BuildingIndex存在，则跳过生成直接调用
    public void LoadMap()
    {
        if(BuildingIndex.Count == 0)
        {
            LevelGenerator lGenerator = new LevelGenerator();
            lGenerator.GridCounts = RowCount * ColumnCount;
            BuildingIndex = lGenerator.FillRoom();
        }

        //放置建筑图标
        for (int i = 0; i < BuildingIndex.Count; i ++)
        {
            if (BuildingIndex[i] != -1)
            {
                m_grid[i].BuildingID = BuildingIndex[i]; //设置Room的 建筑信息
                LoadBuilding(GetPosition(m_grid[i]), BuildingIndex[i]);
            }   
        }

    }

    //放置建筑图标
    private void LoadBuilding(Vector3 position, int buildingId)
    {
        //创建建筑
        BuildingInfo info = Game.Instance.StaticData.GetBuildingInfo(buildingId);
        GameObject go = Game.Instance.ObjectPool.Spawn(info.PrefabName, "prefabs");

        //放置建筑
        Building building = go.GetComponent<Building>();
        building.transform.position = position;

        //TODO: 销毁建筑
        //TODO: 离开场景前需要销毁所有对象，但是保留当前建筑信息

    }

    #endregion

    #region Unity回调
    //只在运行期起作用
    private void Awake()
    {
        //计算地图格子大小
        CalculateSize();

        //调整摄像机视野
        Camera.main.orthographicSize = 7;
  
        //创建所有的格子
        for (int i = 0; i < RowCount; i++)
        {
            for (int j = 0; j < ColumnCount; j++)
            {
                m_grid.Add(new Room(j, i, -1)); //这里出过bug要注意坐标对是(j,i)
            }
        }
    }

    private void Update()
    {
        //鼠标左键检测 点选图标 移动摄像机视角
        if (Input.GetMouseButtonDown(0))
        {
            Room t = GetTileUnderMouse();
            if (t != null)
            {
                //判断事件是否被监听，若true,则触发事件
                TileSelectClickEventArgs e = new TileSelectClickEventArgs(0, t);
                if (OnTileClick != null)
                {
                    OnTileClick(this, e);
                   // Debug.Log("点击左键");
                }
            }
        }

        //鼠标右键检测 打开Mini建筑信息图
        if (Input.GetMouseButtonDown(1))
        {
            Room t = GetTileUnderMouse();
            if(t != null)
            {
                TileSelectClickEventArgs e = new TileSelectClickEventArgs(1, t);
                if(OnTileClick != null)
                {
                    OnTileClick(this, e);
                   // Debug.Log("点击右键");
                }

            }
        }
    }

    //绘制网格 只在编辑器里起作用
    private void OnDrawGizmos()
    {
        if (!DrawGizoms) return;

        CalculateSize();
   
        //绘制行列网格
        Gizmos.color = Color.green;
        for (int row = 0; row <= RowCount; row++)
        {
            Vector2 from = new Vector2(-MapWidth / 2, -MapHeight / 2 + row * TileHeight);
            Vector2 to = new Vector2(-MapWidth / 2 + MapWidth, -MapHeight / 2 + row * TileHeight);
            Gizmos.DrawLine(from, to);
        }

        for (int col = 0; col <= ColumnCount; col++)
        {
            Vector2 from = new Vector2(-MapWidth / 2 + col * TileWidth, MapHeight / 2);
            Vector2 to = new Vector2(-MapWidth / 2 + col * TileWidth, -MapHeight / 2);
            Gizmos.DrawLine(from, to);
        }
    }
    #endregion

    #region 事件回调
    #endregion

    #region 帮助方法
    //计算地图大小，格子大小
    void CalculateSize()
    {
        SpriteRenderer render = GameObject.Find("bg").GetComponent<SpriteRenderer>();
        Vector3 scale = GameObject.Find("Background").GetComponent<Transform>().localScale;   

        MapWidth = render.sprite.bounds.size.x * scale.x;
        MapHeight = render.sprite.bounds.size.y * scale.y;

        ColumnCount = RowCount * (int)(MapWidth / MapWidth); //控制每个格子尽量为正方形，按照背景比例设置行列数量

        TileWidth = MapWidth / ColumnCount;
        TileHeight = MapHeight / RowCount;
    }

    //获取格子中心点世界坐标
    public Vector3 GetPosition(Room t)
    {
        return new Vector3(
            -MapWidth / 2 + (t.X + 0.5f) * TileWidth,
            -MapHeight / 2 + (t.Y + 0.5f) * TileHeight, 0);
    }

    //根据格子索引（X，Y）获得格子
    public Room GetTile(int tileX, int tileY)
    {
        int index = tileX + tileY * ColumnCount;
        if (index < 0 || index >= m_grid.Count)
            return null;


        return m_grid[index];
    }

    //根据所在位置获取格子
    public Room GetTile(Vector3 position)
    {
        int tileX = (int)((position.x + MapWidth / 2) / TileWidth);
        int tileY = (int)((position.y + MapHeight / 2) / TileHeight);
        return GetTile(tileX, tileY); 
    }

    //获取鼠标所在位置的格子坐标（行，列）    
    Room GetTileUnderMouse()
    {
        Vector2 worldPos = GetWorldPosition();
        return GetTile(worldPos);
    }

    //获取鼠标所在位置的世界坐标
    Vector3 GetWorldPosition()
    {
        Vector3 viewPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Vector3 worldPos = Camera.main.ViewportToWorldPoint(viewPos);
        return worldPos;
    }
    #endregion
}
