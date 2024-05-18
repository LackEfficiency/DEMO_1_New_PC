using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIStore : View
{
    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    //卡牌信息文件
    TextAsset m_CardData;

    //用游戏模型储存卡牌信息 
    GameModel m_GameModel;
    #endregion

    #region 属性
    public override string Name
    {
        get { return Consts.V_Store; }
    }

    public GameModel GameModel
    { 
        get => m_GameModel; 
    }

    #endregion

    #region 方法
    public void LoadCardData()
    {
        //拆分成多列
        string[] dataRow = m_CardData.text.Split("\n");

        //拆分列内内容
        foreach (string row in dataRow)
        {
            string[] rowArray = row.Split(",");
            if (rowArray[0] == "#")
            {
                continue;
            }
            else if (rowArray[0] == "monster")
            {
                //新建怪兽卡
                int id = int.Parse(rowArray[1]);
                string name = rowArray[2];
                int atk = int.Parse(rowArray[3]);
                int hp = int.Parse(rowArray[4]);
                int cost = int.Parse(rowArray[5]);
                MonsterCardInfo monsterCard = new MonsterCardInfo(CardType.Monster, id, name, cost, atk, hp);
                
                GameModel.Cards.Add(monsterCard);
            }
            else if (rowArray[0] == "spell")
            {
                //新建魔法卡
                int id = int.Parse(rowArray[1]);
                string name = rowArray[2];
                string effect= rowArray[3];
                int cost = int.Parse(rowArray[4]);
                SpellCardInfo spellCard = new SpellCardInfo(CardType.Spell, id, name, cost, effect);

                GameModel.Cards.Add(spellCard);
            }
        }

        //初始化playermodel所有卡牌的长度
        PlayerModel pm = GetModel<PlayerModel>();
        pm.m_Library = new int[GameModel.CardCount];
        pm.m_Deck = new int[GameModel.CardCount];

        //初始化pm,从存档里加入
        pm.LoadFileData();
    }

    public CardInfo RandomCard() //随机抽一张卡
    {
        CardInfo cardInfo = GameModel.Cards[UnityEngine.Random.Range(0, GameModel.CardCount)];
        return cardInfo;
    }
    #endregion

    #region Unity回调
    #endregion

    #region 事件回调
    public override void RegisterEvents()
    {
        AttentionEvents.Add(Consts.E_EnterScene); 
    }

    public override void HandleEvent(string eventName, object data = null)
    {
        switch (eventName)
        {
            case Consts.E_EnterScene:
                SceneArgs e0 = data as SceneArgs;
                if (e0.SceneIndex == 1)
                {
                    m_GameModel = GetModel<GameModel>();
                    if (m_GameModel.Cards.Count == 0) //仅当第一次进入场景时调用
                    {
                        m_CardData = Resources.Load<TextAsset>(Consts.CardDataDir + "/CardList");
                        LoadCardData();
                    }
                }
                break;
            default: break;
        }
    }
    #endregion

    #region 帮助方法
    #endregion
}
