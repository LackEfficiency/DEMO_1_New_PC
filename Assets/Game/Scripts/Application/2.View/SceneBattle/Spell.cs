using System;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

//管理法术的使用
public class Spell : View
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

    //判断是否正在使用中
    bool is_Spelling = false;  

    //等待使用的法术
    GameObject m_WaitingSpell = null;

    //获取召唤控制视图
    Spawner m_Spawner = null;

    //显示可以作用法术的位置
    GameObject m_SpellIcon = null;

    //可以作用法术的位置
    Vector3 m_SpellPos;

    #endregion

    #region 属性
    public override string Name
    {
        get { return Consts.V_Spell; }
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
    public bool Is_Spelling { get => is_Spelling; set => is_Spelling = value; }
    public GameObject WaitingSpell { get => m_WaitingSpell; set => m_WaitingSpell = value; }
    public GameObject SpellIcon { get => m_SpellIcon; set => m_SpellIcon = value; }
    public Vector3 SpellPos { get => m_SpellPos; set => m_SpellPos = value; }

    #endregion

    #region 方法
    public void UsingSpell(SpellCardInfo spellCardInfo, TileBattle tileBattle, Player player)
    {
        //己方使用法术
        if (player == Player.Self)
        {
            MonsterCard targetCard = tileBattle.Card.GetComponent<Card>() as MonsterCard;
            //对法术描述按空格切片，得到法术的具体细节（效果，伤害值等）
            string[] effectDetails = spellCardInfo.Effect.Split(' ');
            //0是法术效果
            string effectName = effectDetails[0];

            //区分一次性法术和Buff法术
            if (Consts.EffectNames.Contains(effectName))
            {
                //根据法术效果进行处理
                Effect effect = Game.Instance.EffectManager.GetEffect(effectName);           
                effect.Cast(targetCard);
            }
            //Buff使用Buff系统
            else
            {
                //Buff法术
                int BuffRound = int.Parse(effectDetails[1]);

               //根据Buff效果添加Buff
                BuffBase buff = Game.Instance.BuffManager.GetBuff(effectName);
                Game.Instance.BuffManager.AddBuffToMonster(targetCard, buff);
            }


        }

        //TODO:敌人使用法术
    }

        #endregion

        #region Unity回调
        #endregion

        #region 事件回调
    public override void RegisterEvents()
    {
        AttentionEvents.Add(Consts.E_EnterScene); //为了获取当前map脚本，从而导入地图信息
        AttentionEvents.Add(Consts.E_UseSpellCardRequest); //监听法术卡的使用请求 
        AttentionEvents.Add(Consts.E_ConfirmSpell); //监听法术的确认使用
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

                    //初始化召唤控制视图
                    m_Spawner = GetComponent<Spawner>();

                }
                break;

            case Consts.E_UseSpellCardRequest:
                UseSpellCardRequestArgs e1 = data as UseSpellCardRequestArgs;

                //切换类型 召唤->法术
                m_Spawner.Is_Summon = false;
                m_Spawner.WaitingSummon = null;

                //判断是否点击卡牌后重复点击卡牌而不是格子，is_Spelling判断是否处于法术选择对象过程中
                if (Is_Spelling)
                {
                    WaitingSpell = e1.go;
                    return;
                }
                Is_Spelling = true;

                //找到所有存在卡牌的格子，判断卡牌是否是使用对象（己方还是敌方）
                foreach (TileBattle tile in m_Map.Grid)
                {   
                    //法术有多种类型，根据作用对象不同，分别处理
                    //这里暂时只处理对单位的法术 Self, Enemy, Any
                    if (tile.Card)
                    {
                        //获取该格子的卡牌控制者
                        SpellCardInfo spellCardInfo = e1.cardInfo as SpellCardInfo;
                        Player player = tile.Card.GetComponent<Card>().Player;
                        SpellPos = m_Map.GetPosition(tile);

                        switch (spellCardInfo.SpellType)
                        {
                            //控制者为己方 并且卡牌的作用对象也是己方
                            case SpellType.Self:
                                if (player == Player.Self)
                                {
                                    //显示可以释放
                                    SpellIcon = Game.Instance.ObjectPool.Spawn("SpellSelect", "prefabs/others");
                                    //己方设置为蓝色
                                    SpellIcon.GetComponent<SpriteRenderer>().color = Color.blue;

                                    //这里不复用代码是因为存在该情况，当使用对象不存在时，switch会直接跳出，不会执行后续代码，
                                    //如果在switch语句外进行位置的设置，会导致位置错误
                                    SpellIcon.transform.position = SpellPos;
                                    tile.Data = SpellIcon;

                                    //至少找到一个单位允许释放时，允许释放
                                    if (WaitingSpell == null)
                                    {
                                        WaitingSpell = e1.go;
                                        //每次释放时开始监听 完成释放后回收监听
                                        m_Map.OnTileClick += Spell_OnTileClick;
                                    }
                                }   
                                break;
                            
                            //控制者为敌方 并且卡牌的作用对象也是敌方
                            case SpellType.Enemy:
                                if (player == Player.Enemy)
                                {
                                    //显示可以释放
                                    SpellIcon = Game.Instance.ObjectPool.Spawn("SpellSelect", "prefabs/others");
                                    //敌方设置为红色
                                    SpellIcon.GetComponent<SpriteRenderer>().color = Color.red;
                                    SpellIcon.transform.position = SpellPos;
                                    tile.Data = SpellIcon;

                                    //至少找到一个单位允许释放时，允许释放
                                    if (WaitingSpell == null)
                                    {
                                        WaitingSpell = e1.go;
                                        //每次释放时开始监听 完成释放后回收监听
                                        m_Map.OnTileClick += Spell_OnTileClick;
                                    }
                                }   
                                break;

                            //卡牌的作用对象为任意单位
                            case SpellType.Any:
                                //显示可以释放
                                SpellIcon = Game.Instance.ObjectPool.Spawn("SpellSelect", "prefabs/others");
                                //设置为黄色
                                SpellIcon.GetComponent<SpriteRenderer>().color = Color.yellow;
                                SpellIcon.transform.position = SpellPos;
                                tile.Data = SpellIcon;

                                //至少找到一个单位允许释放时，允许释放
                                if (WaitingSpell == null)
                                {
                                    WaitingSpell = e1.go;
                                    //每次释放时开始监听 完成释放后回收监听
                                    m_Map.OnTileClick += Spell_OnTileClick;
                                }
                                break;

                            default:
                                break;
                        }
                    }
                }
                break;

            case Consts.E_ConfirmSpell:

                ConfirmSpellArgs e2 = data as ConfirmSpellArgs;
                //使用法术
                UsingSpell(WaitingSpell.GetComponent<UICard>().CardInfo as SpellCardInfo, e2.tile, e2.player);

                //销毁手牌中的卡
                if (e2.player == Player.Self)
                {
                    //手牌移除这张卡
                    RoundModel.PlayerHandList.Remove(WaitingSpell);

                    //销毁实体
                    WaitingSpell.GetComponent<Card>().PosState = true;

                    //数据更新               
                    WaitingSpell = null;
                }

                break;

            default:
                break;  
        }
    }

    private void Spell_OnTileClick(object sender, TileBattleClickEventArgs e)
    {
        TileBattle tile = e.tile;

        //取消使用法术 使用Spawner的CancelSummon方法
        if (e.MouseButton == 1)
        {
            SendEvent(Consts.E_CancelSummon);
            return;
        }

        //确认可以使用法术
        if (tile.Data != null && Is_Spelling)
        {
            ConfirmSpellArgs e1 = new ConfirmSpellArgs();
            e1.tile = tile;
            e1.player = Player.Self;

            SendEvent(Consts.E_ConfirmSpell, e1);
            //完成使用
            SendEvent(Consts.E_CancelSummon);
        }
    }
    #endregion

    #region 帮助方法
    #endregion


}