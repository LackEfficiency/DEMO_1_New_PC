﻿using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Spawner : View
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
    bool is_Summon = false;

    GameObject m_WaitingSummon = null;
    #endregion

    #region 属性
    public override string Name
    {
        get { return Consts.V_Spawner; }
    }

    public RoundModel RoundModel
    { 
        get => rModel; 
    }

    public PlayerModel PlayerModel
    {
        get => pModel;
    }

    public GameModel GameModel
    {
        get => gModel;
    }

    #endregion

    #region 方法
    void MoveFromField(Card card)
    {
        if (card != null && card.gameObject != null)
        {
            Game.Instance.ObjectPool.Unspawn(card.gameObject);
        }
    }

    void Summon(CardInfo cardInfo, TileBattle tile, Player player)
    {
        //TODO: 新建召唤对象
        GameObject go = Game.Instance.ObjectPool.Spawn("HeavyBandit", "prefabs/Character"); //新建召唤物对象
        Card m_card = go.GetComponent<Card>();
        m_card.CardPosistionChange += MoveFromField;
        m_card.Player = player;
       
        Vector3 pos = m_Map.GetPosition(tile);
        m_card.transform.position = pos;

        UIUnitStatus uIUnitStatus = go.GetComponent<UIUnitStatus>();
        uIUnitStatus.CardInfo = cardInfo; //召唤对应的卡
        uIUnitStatus.Show();
        

        if (player == Player.Self) //如果是玩家 则朝向右
        {
            // 获取render组件
            SpriteRenderer mySpriteRenderer = go.GetComponent<SpriteRenderer>();

            // 左右翻转
            mySpriteRenderer.flipX = true;
        }
        tile.Card = go;

        //场上加入这张卡
        if (player == Player.Self)
        {
            RoundModel.PlayerSummonList.Add(go);
        }
        else if (player == Player.Enemy)
        {
            RoundModel.EnemySummonList.Add(go);
        }
    }
    #endregion

    #region Unity回调

    #endregion

    #region 事件回调
    public override void RegisterEvents() 
    {
        AttentionEvents.Add(Consts.E_EnterScene); //为了获取当前map脚本，从而导入地图信息
        AttentionEvents.Add(Consts.E_SummonCardRequest);
        AttentionEvents.Add(Consts.E_CancelSummon);
        AttentionEvents.Add(Consts.E_ConfirmSummon);
        AttentionEvents.Add(Consts.E_EnemySummon);
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
                    m_Map.LoadLevel(GameModel.PlayLevel);

                }
                break;

            case Consts.E_SummonCardRequest:
                SummonCardRequestArgs e1 = data as SummonCardRequestArgs;

                //判断是否点击卡牌后重复点击卡牌而不是格子，is_Summon判断是否处于召唤过程中
                if (is_Summon)
                {
                    m_WaitingSummon = e1.go;
                    return;
                }
                is_Summon = true;

                foreach (TileBattle tile in m_Map.Grid)
                {
                    if (tile.Card == null && tile.CanSetMe)
                    {
                        //显示可以召唤
                        Vector3 m_pos = m_Map.GetPosition(tile);
                        GameObject SpawnPos = Game.Instance.ObjectPool.Spawn("Area", "prefabs/others");
                        SpawnPos.transform.position = m_pos;
                        tile.Data = SpawnPos;
                    }
                }
                m_WaitingSummon = e1.go;
                //每次召唤时开始监听 完成召唤后回收监听
                m_Map.OnTileClick += Map_OnTileClick;
                break;

            case Consts.E_CancelSummon: //右键取消召唤
                m_WaitingSummon = null;
                is_Summon = false;
                //取消召唤时也回收监听
                m_Map.OnTileClick -= Map_OnTileClick;
                foreach(TileBattle tile in m_Map.Grid)
                {
                    if (tile.Data != null)
                    {
                        Game.Instance.ObjectPool.Unspawn(tile.Data as GameObject);
                        tile.Data = null;
                    }
                }
                break;

            case Consts.E_ConfirmSummon: //确认召唤

                ConfirmSummonArgs e2 = data as ConfirmSummonArgs;
                Summon(m_WaitingSummon.GetComponent<UICard>().CardInfo, e2.tile, Player.Self);

                //销毁手牌中的卡
                if (e2.player == Player.Self)
                {
                    //手牌移除这张卡
                    RoundModel.PlayerHandList.Remove(m_WaitingSummon);

                    //销毁实体
                    m_WaitingSummon.GetComponent<Card>().PosState = true;
                 
                    //数据更新               
                    m_WaitingSummon = null;


                }
                foreach (TileBattle tile in m_Map.Grid)
                {
                    if (tile.Data != null)
                    {
                        Game.Instance.ObjectPool.Unspawn(tile.Data as GameObject);
                        tile.Data = null;
                    }
                }
                break;

            case Consts.E_EnemySummon: //敌人召唤

                EnemySummonArgs e3 = data as EnemySummonArgs;

                //随机取可召唤格子
                List<int> shuffleGrid = Tools.GenerateSequence(m_Map.Grid.Count);
                Tools.ShuffleList(shuffleGrid);

                foreach (int tileID in shuffleGrid)
                {
                    //有空的格子则可以召唤
                    if (m_Map.Grid[tileID].CanSetEnemy == true && m_Map.Grid[tileID].Card == null)
                    {
                        Summon(e3.cardInfo, m_Map.Grid[tileID], Player.Enemy);

                        //数据更新
                        //手牌移除这张卡
                        RoundModel.EnemyHandList.Remove(e3.cardInfo);
                        break;
                    }
                    
                }
                break;

            default: break;
        }
    }

    private void Map_OnTileClick(object sender, TileBattleClickEventArgs e)
    {
        TileBattle tile = e.tile;
        if (e.MouseButton == 1) //1时取消放置
        {
            SendEvent(Consts.E_CancelSummon);
            return;
        }
        if (tile.Card == null && tile.CanSetMe)
        {
            //TODO: 召唤卡牌 
            ConfirmSummonArgs e1 = new ConfirmSummonArgs();
            e1.tile = e.tile;
            e1.player = Player.Self;
            SendEvent(Consts.E_ConfirmSummon, e1);
            //完成召唤
            SendEvent(Consts.E_CancelSummon);
        }
    }
    #endregion

    #region 帮助方法
    #endregion
}