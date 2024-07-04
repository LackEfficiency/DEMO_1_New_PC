using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIButtonBattle : View
{
    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    public Button btnTurnEnd; //当前阶段结束按钮
    public Button btnRemoveUnit; //移除单位按钮
    MapBattle m_Map = null; //地图
    RoundModel rModel = null; 

    private bool isShovelMode = false; //是否是铲子模式
    public Texture2D shovelCursor; //铲子材质
    #endregion

    #region 属性
    public override string Name
    {
        get { return Consts.V_ButtonBattle; }
    }
    #endregion

    #region 方法
    #endregion

    #region Unity回调
    private void Map_OnRemoveClick(object sender, TileBattleClickEventArgs e)
    {
        //取消铲子模式需要再点击按钮
        TileBattle tile = e.tile;
        if (tile.Card != null && isShovelMode)
        {
            //是否是自己的卡
            MonsterCard card = tile.Card.GetComponent<MonsterCard>();
            if (card.player == Player.Self)
            {
                //删除卡牌
                if (card != null)
                {
                    card.Kill();
                }
            }
        }

    }
    public void OnClickEnd()
    {
        SendEvent(Consts.E_TurnEnd);
    }

    public void OnClickRemove()
    {
        //鼠标变为铲子
        isShovelMode = !isShovelMode;
        if (isShovelMode)
        {
            Cursor.SetCursor(shovelCursor, Vector2.zero, CursorMode.Auto);
            //开始监听地图点击事件
            m_Map.OnTileClick += Map_OnRemoveClick;
        }
        else
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            //移除监听
            m_Map.OnTileClick -= Map_OnRemoveClick;
        }
    }
    #endregion

    #region 事件回调
    public override void RegisterEvents()
    {
        base.RegisterEvents();
        AttentionEvents.Add(Consts.E_EnterScene);
    }

    public override void HandleEvent(string eventName, object data = null)
    {
        switch (eventName)
        {
            case Consts.E_EnterScene:
                SceneArgs e = data as SceneArgs;
                if (e.SceneIndex == 3)
                {
                    m_Map = GameObject.Find("Map").GetComponent<MapBattle>();
                    rModel = GetModel<RoundModel>();
                }
                break;
            default:
                break;
        }
    }
    #endregion

    #region 帮助方法
    #endregion
}
