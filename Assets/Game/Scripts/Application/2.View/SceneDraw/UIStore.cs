using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIStore : View
{
    #region ����
    #endregion

    #region �¼�
    #endregion

    #region �ֶ�
    //������Ϣ�ļ�
    TextAsset m_CardData;

    //����Ϸģ�ʹ��濨����Ϣ 
    GameModel m_GameModel;
    #endregion

    #region ����
    public override string Name
    {
        get { return Consts.V_Store; }
    }

    public GameModel GameModel
    { 
        get => m_GameModel; 
    }

    #endregion

    #region ����
    public void LoadCardData()
    {
        //��ֳɶ���
        string[] dataRow = m_CardData.text.Split("\n");

        //�����������
        foreach (string row in dataRow)
        {
            string[] rowArray = row.Split(",");
            if (rowArray[0] == "#")
            {
                continue;
            }
            else if (rowArray[0] == "monster")
            {
                //�½����޿�
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
                //�½�ħ����
                int id = int.Parse(rowArray[1]);
                string name = rowArray[2];
                string effect= rowArray[3];
                SpellType spellType = (SpellType)Enum.Parse(typeof(SpellType), rowArray[4]);
                int cost = int.Parse(rowArray[5]);
                SpellCardInfo spellCard = new SpellCardInfo(CardType.Spell, id, name, cost, effect, spellType);

                GameModel.Cards.Add(spellCard);
            }
        }

        //��ʼ��playermodel���п��Ƶĳ���
        PlayerModel pm = GetModel<PlayerModel>();
        pm.m_Library = new int[GameModel.CardCount];
        pm.m_Deck = new int[GameModel.CardCount];

        //��ʼ��pm,�Ӵ浵�����
        pm.LoadFileData();
    }

    public CardInfo RandomCard() //�����һ�ſ�
    {
        CardInfo cardInfo = GameModel.Cards[UnityEngine.Random.Range(0, GameModel.CardCount)];
        return cardInfo;
    }
    #endregion

    #region Unity�ص�
    #endregion

    #region �¼��ص�
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
                    if (m_GameModel.Cards.Count == 0) //������һ�ν��볡��ʱ����
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

    #region ��������
    #endregion
}
