using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

//管理卡牌的召唤   
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

    Spell m_Spell = null;
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
    public GameObject WaitingSummon { get => m_WaitingSummon; set => m_WaitingSummon = value; }
    public bool Is_Summon { get => is_Summon; set => is_Summon = value; }

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
        MonsterCard m_card = go.GetComponent<MonsterCard>();

        m_card.MonsterCardInfo = cardInfo as MonsterCardInfo;
        //每次召唤时更新状态
        m_card.UpdateStats();
        m_card.CardPosistionChange += MoveFromField;
        m_card.Dead += Card_Dead;
        m_card.StatusChanged += UpdateStatus;
        m_card.Player = player;

        //订阅Buff事件
        m_card.OnAttack += (MonsterCard) => gModel.BuffManager.TriggerEvent(m_card, BuffEvent.OnAttack);
        m_card.OnTakeDamage += (MonsterCard) => gModel.BuffManager.TriggerEvent(m_card, BuffEvent.OnTakeDamage);
        m_card.OnActionFinish += (MonsterCard) => gModel.BuffManager.TriggerEvent(m_card, BuffEvent.OnActionFinish);
        m_card.OnDie += (MonsterCard) => gModel.BuffManager.TriggerEvent(m_card, BuffEvent.OnDie);
        m_card.OnMove += (MonsterCard) => gModel.BuffManager.TriggerEvent(m_card, BuffEvent.OnMove);
        m_card.OnTakeHeal += (MonsterCard) => gModel.BuffManager.TriggerEvent(m_card, BuffEvent.OnTakeHeal);
        m_card.OnActionFinish += (MonsterCard) => gModel.BuffManager.OnActionFinish(m_card);


        Vector3 pos = m_Map.GetPosition(tile);
        m_card.transform.position = pos;

        //显示卡牌信息 每次状态变化时都需要更新
        UpdateStatus(m_card);

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

    private void M_card_OnActionFinish(MonsterCard obj)
    {
        throw new NotImplementedException();
    }

    void Card_Dead(Card card)
    {
        //从场上移除这张卡
        if (card.player == Player.Self)
        {
            RoundModel.PlayerSummonList.Remove(card.gameObject);
        }
        else if (card.player == Player.Enemy)
        {
            RoundModel.EnemySummonList.Remove(card.gameObject);
        }

        //格子清空
        foreach (TileBattle tile in m_Map.Grid)
        {
            if (tile.Card == card.gameObject)
            {
                tile.Card = null;
            }
        }   

        //销毁卡牌
        Game.Instance.ObjectPool.Unspawn(card.gameObject);
        
    }

    //展示信息
    public void UpdateStatus(MonsterCard card)
    {
        UIUnitStatus uIUnitStatus = card.GetComponent<UIUnitStatus>();
        uIUnitStatus.Card = card;
        uIUnitStatus.Show();
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

                    //获取法术控制视图
                    m_Spell = GetComponent<Spell>();

                }
                break;

            case Consts.E_SummonCardRequest:
                SummonCardRequestArgs e1 = data as SummonCardRequestArgs;

                //切换类型 法术->召唤
                m_Spell.Is_Spelling = false;
                m_Spell.WaitingSpell = null;

                //判断是否点击卡牌后重复点击卡牌而不是格子，is_Summon判断是否处于召唤过程中
                if (Is_Summon)
                {
                    WaitingSummon = e1.go;
                    return;
                }
                Is_Summon = true;

                foreach (TileBattle tile in m_Map.Grid)
                {
                    if (tile.Card == null && tile.CanSetMe)
                    {
                        //显示可以召唤
                        Vector3 m_pos = m_Map.GetPosition(tile);
                        GameObject SpawnPos = Game.Instance.ObjectPool.Spawn("Area", "prefabs/others");
                        SpawnPos.transform.position = m_pos;
                        tile.Data = SpawnPos;

                        //至少找到一个格子可以放置时，允许放置
                        if (WaitingSummon == null)
                        {
                            WaitingSummon = e1.go;
                            //每次召唤时开始监听 完成召唤后回收监听
                            m_Map.OnTileClick += Map_OnTileClick;
                        }
                    }
                }
                break;

            case Consts.E_CancelSummon: //右键取消召唤

                //重置召唤状态
                WaitingSummon = null;
                Is_Summon = false;

                m_Spell.WaitingSpell = null;
                m_Spell.Is_Spelling = false;

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
                Summon(WaitingSummon.GetComponent<UICard>().CardInfo, e2.tile, Player.Self);

                //销毁手牌中的卡
                if (e2.player == Player.Self)
                {
                    //手牌移除这张卡
                    RoundModel.PlayerHandList.Remove(WaitingSummon);

                    //销毁实体
                    WaitingSummon.GetComponent<Card>().PosState = true;
                 
                    //数据更新               
                    WaitingSummon = null;
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
        if (tile.Card == null && tile.CanSetMe && Is_Summon)
        {
            ConfirmSummonArgs e1 = new ConfirmSummonArgs();
            e1.tile = tile;
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