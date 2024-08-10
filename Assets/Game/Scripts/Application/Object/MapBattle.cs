using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


//用于描述一个关卡地图的状态
public class MapBattle : MonoBehaviour
{
    #region 常量
    public const int RowCount = 6; //行数
    public const int ColumnCount = 12; //列数

    #endregion

    #region 事件
    public event EventHandler<TileBattleClickEventArgs> OnTileClick;
    #endregion

    #region 字段
    float MapWidth; //地图宽
    float MapHeight; //地图高

    float TileWidth; //每一个格子的宽
    float TileHeight; //每一个格子的高

    public bool DrawGizoms = true; //是否显示网格

    List<TileBattle> m_grid = new List<TileBattle>(); //格子集合

    Level m_level; //关卡数据

    #endregion

    #region 属性
    public Level Level { get { return m_level; } }
    //根据图片地址加载图片
    public string BackgroundImage
    {
        set
        {
            SpriteRenderer render = transform.Find("Background").GetComponent<SpriteRenderer>();
            StartCoroutine(Tools.LoadImage(value, render));
        }
    }

    public string RoadImage
    {
        set
        {
            SpriteRenderer render = transform.Find("Road").GetComponent<SpriteRenderer>();
            StartCoroutine(Tools.LoadImage(value, render));
        }
    }

    public List<TileBattle> Grid
    {
        get { return m_grid; }
    }

    #endregion

    #region 方法
    public void LoadLevel(Level level)
    {
        //清空当前状态
        Clear();

        //保存关卡信息
        m_level = level;

        //设置图片
        BackgroundImage = "file://" + Consts.MapDir  + "/" + level.Background;
        RoadImage = "file://" + Consts.MapDir + "/" + level.Road;

        //单位站位位置
        for (int i = 0; i < level.Holder.Count; i++)
        {
            Point p = level.Holder[i];
            TileBattle t = GetTile(p.X, p.Y);
            t.CanHold = true;
        }

        //敌方单位放置位置
        for (int i = 0; i < level.Set.Count; i++)
        {
            Point p = level.Set[i];
            TileBattle t = GetTile(p.X, p.Y);
            t.CanSetEnemy = true;
        }

        //己方单位放置位置
        for (int i = 0; i < level.Set.Count; i++)
        {
            Point p = level.Set[i];
            TileBattle t = GetTile(ColumnCount-1-p.X, p.Y);
            t.CanSetMe = true;
        }

    }
    #endregion

    //清除单位是否进入信息
    public void ClearHolder()
    {
        foreach (TileBattle t in m_grid)
        {
            if (t.CanHold)
            {
                t.CanHold = false;
            }
        }
    }

    //清除单位是否可放置信息
    public void ClearSet()
    {
        foreach(TileBattle t in m_grid)
        {
            if (t.CanSetEnemy)
            {
                t.CanSetEnemy = false;
            }

            if (t.CanSetMe)
            {
                t.CanSetMe = false;
            }
        }
    }

    //清除所有信息
    public void Clear()
    {
        m_level = null;
        ClearHolder();
        ClearSet();
    }

    #region Unity回调
    //只在运行期起作用
    private void Awake()
    {
        //计算地图格子大小
        CalculateSize();

        //创建所有的格子
        for (int i = 0; i < RowCount; i++)
        {
            for (int j = 0; j < ColumnCount; j++)
            {
                m_grid.Add(new TileBattle(j, i)); //这里出过bug要注意坐标对是(j,i)
            }
        }

        //监听鼠标点击事件
        OnTileClick += Map_OnTileClick;

    }

    private void Update()
    {
        //鼠标左键检测
        if (Input.GetMouseButtonDown(0))
        {
            TileBattle t = GetTileUnderMouse();
            if (t != null)
            {
                //判断事件是否被监听，若true,则触发事件
                TileBattleClickEventArgs e = new TileBattleClickEventArgs(0, t);
                if (OnTileClick != null)
                {
                    OnTileClick(this, e);
                   // Debug.Log("点击左键");
                }
            }
        }

        //鼠标右键检测
        if (Input.GetMouseButtonDown(1))
        {
            TileBattle t = GetTileUnderMouse();
            if (t != null)
            {
                //判断事件是否被监听，若true,则触发事件
                TileBattleClickEventArgs e = new TileBattleClickEventArgs(1, t);
                if (OnTileClick != null)
                {
                    OnTileClick(this, e);
                    Debug.Log("点击右键");
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

        //绘制单位可进入网格
        foreach (TileBattle t in m_grid)
        {
            if (t.CanHold)
            {
                Vector3 pos = GetPosition(t);
                Gizmos.DrawIcon(pos, "holder.png", true);
            }
        }

        //绘制放置单位网格,
        foreach (TileBattle t in m_grid)
        {
            if (t.CanSetMe)
            {
                Gizmos.color = Color.blue;
                Vector3 pos = GetPosition(t);
                Gizmos.DrawSphere(pos, 0.5f);
            }
            if (t.CanSetEnemy)
            {
                Gizmos.color = Color.red;
                Vector3 pos = GetPosition(t);
                Gizmos.DrawSphere(pos, 0.5f);
            }
        }

    }
    #endregion

    #region 事件回调
    void Map_OnTileClick(object sender, TileBattleClickEventArgs e)
    {
        //当前场景不是LevelBuilder则不能编辑


        if (gameObject.scene.name != "LevelBuilder")
            return;

        if (Level == null) return;

        //处理左键事件，放置或取消可进入点
        if (e.MouseButton == 0)
        {
            e.tile.CanHold = !e.tile.CanHold;
        }

        //处理右键事件， 设置或取消可放置点
        if (e.MouseButton == 1)
        {
            e.tile.CanSetEnemy = !e.tile.CanSetEnemy;
            int MeX = ColumnCount -1 - e.tile.X;    
            int MeY = e.tile.Y;
            TileBattle t = GetTile(MeX, MeY);
            t.CanSetMe = !t.CanSetMe;
        } 
    }
    #endregion

    #region 帮助方法
    //计算地图大小，格子大小
    void CalculateSize()
    {
        Vector3 leftdown = new Vector3(0, 0);
        Vector3 rightup = new Vector3(1, 1);
        Vector3 p1 = Camera.main.ViewportToWorldPoint(leftdown);
        Vector3 p2 = Camera.main.ViewportToWorldPoint(rightup);

        MapWidth = (p2.x - p1.x);
        MapHeight = (p2.y - p1.y);

        TileWidth = MapWidth / ColumnCount;
        TileHeight = MapHeight / RowCount;
    }

    //获取格子中心点世界坐标
    public Vector3 GetPosition(TileBattle t)
    {
        return new Vector3(
            -MapWidth / 2 + (t.X + 0.5f) * TileWidth,
            -MapHeight / 2 + (t.Y + 0.5f) * TileHeight, 0);
    }

    //根据格子索引（X，Y）获得格子
    public TileBattle GetTile(int tileX, int tileY)
    {
        int index = tileX + tileY * ColumnCount;
        if (index < 0 || index >= m_grid.Count)
            return null;


        return m_grid[index];
    }

    //根据所在位置获取格子
    public TileBattle GetTile(Vector3 position)
    {
        int tileX = (int)((position.x + MapWidth / 2) / TileWidth);
        int tileY = (int)((position.y + MapHeight / 2) / TileHeight);
        return GetTile(tileX, tileY); 
    }

    //获取鼠标所在位置的格子坐标（行，列）    
    TileBattle GetTileUnderMouse()
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
