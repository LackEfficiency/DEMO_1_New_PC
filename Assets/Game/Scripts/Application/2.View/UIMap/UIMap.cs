using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UIMap : View
{
    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段

    MapSelect m_Map = null;

    int m_SelectedIndex = -1; //当打开某个关卡时，提示信息通过选择索引进行更新

    GameModel m_GameModel = null;
    #endregion

    #region 属性
    public override string Name
    {
        get { return Consts.V_Select; }
    }
    #endregion

    #region 方法
    #endregion

    #region Unity回调
    #endregion

    #region 事件回调
    public override void RegisterEvents()
    {
        AttentionEvents.Add(Consts.E_EnterScene); //为了获取当前map脚本，从而导入地图信息
    }


    private void Map_OnTileClick(object sender, TileSelectClickEventArgs e)
    {
        GameModel gm = GetModel<GameModel>();

        if (gm.IsPlaying && e.tile.BuildingID != -1)
        {
            Room room = e.tile;

            //左键事件 移动摄像机视角
            if (e.MouseButton == 0)
            {
                MoveCamera(e.tile);
            }

            //右键事件 弹出面板
            if (e.MouseButton == 1)
            {
                SendEvent(Consts.E_ShowBattlePanel);
                gm.IsPlaying = false;
            }
        }
    }

    public override void HandleEvent(string eventName, object data = null)
    {
        switch (eventName)
        {
            case Consts.E_EnterScene:
                SceneArgs e0 = data as SceneArgs;
                if (e0.SceneIndex == 2)
                {
                    //获取数据
                    GameModel gModel = GetModel<GameModel>();

                    //加载地图
                    m_Map = GetComponent<MapSelect>();
                    m_Map.OnTileClick += Map_OnTileClick;
                    m_Map.LoadMap();
                }
                break;
        }
    }
    #endregion

    #region 帮助方法
    //移动摄像机
    void MoveCamera(Room tile)
    {
        float CameraSize = Camera.main.orthographicSize;
        Vector3 CameraPos = m_Map.GetPosition(tile);

        //如果摄像机视野超出地图边界，不移动视角
        SpriteRenderer render = GameObject.Find("border").GetComponent<SpriteRenderer>();
        Vector3 scale = GameObject.Find("Background").GetComponent<Transform>().localScale;

        float BorderWidth = render.sprite.bounds.size.x * scale.x;
        float BorderHeight = render.sprite.bounds.size.y * scale.y;
        float WoverH = BorderWidth / BorderHeight;

        if ((((CameraPos.x + CameraSize + 0.3) * WoverH) > (BorderWidth / 2)) || ((Math.Abs(CameraPos.x - CameraSize - 0.3) * WoverH) > (BorderWidth / 2)))
            return;
        if (((CameraPos.y + CameraSize + 0.3) > BorderHeight / 2) || (Math.Abs(CameraPos.y - CameraSize - 0.3) > BorderHeight / 2))
            return;


        CameraPos.z = -10;
        Camera.main.transform.position = CameraPos;
    }
    #endregion
}

