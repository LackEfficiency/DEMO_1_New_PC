using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerModel : Model //存储游戏数据
{
    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    //卡牌信息文件
    TextAsset m_PlayerData;

    //所有的卡牌
    public int[] m_Library;

    //当前卡组
    public int[] m_Deck;

    //当前金币
    int m_Coins;
    #endregion

    #region 属性
    public override string Name
    {
        get { return Consts.M_PlayerModel; }
    }

    public int[] Library 
    {
        get { return m_Library; }
    }

    public int[] Deck
    {
        get { return m_Deck; }
    }

    public int Coins 
    {
        get { return m_Coins; }
        set { m_Coins = value; }
    }

    public TextAsset PlayerData
    {
        get { return m_PlayerData; }
    }

    #endregion

    #region 方法
    public void Initialize() //scene0里调用初始化
    {
        //m_cards的初始化需要得到所有卡牌的长度，因此在UIStore里完成
        m_Coins = 20;
    }

    public void SaveCardData(int CardID) //储存数据至model
    {
        m_Library[CardID] += 1;
    }

    public void SaveDeckData(int CardD) //储存数据至Model
    {
        m_Deck[CardD] += 1;
    }

    public void SaveDataToFile() //储存数据至文件
    {
        string path = Application.dataPath + @"/Game/Resources/" + Consts.CardDataDir + @"/PlayerData.csv";

        //保存卡牌
        List<string> datas = new List<string>();
        datas.Add("coins," + Coins.ToString());
        for(int i = 0; i < Library.Length; i++)
        {
            if (Library[i] != 0)
                datas.Add("card," + i.ToString() + "," + Library[i].ToString());
        }
        //保存卡组
        for (int i = 0; i < Deck.Length; i++)
        {
            if (Deck[i] != 0)
                datas.Add("deck," + i.ToString() + "," + Deck[i].ToString());
        }

        //保存数据
        File.WriteAllLines(path, datas);
    }

    public void LoadFileData() //加载储存的数据
    {
        m_PlayerData = Resources.Load<TextAsset>(Consts.CardDataDir + "/PlayerData");
        string[] dataRow = PlayerData.text.Split('\n');
        foreach (var row in dataRow)
        {
            string[] rowArray = row.Split(',');
            if ( rowArray[0] == "#")
            {
                continue;
            }
            else if (rowArray[0] == "coins")
            {
                m_Coins = int.Parse(rowArray[1]);
            }
            else if (rowArray[0] == "card")
            {
                int id = int.Parse(rowArray[1]);
                int num = int.Parse(rowArray[2]);
                Library[id] = num;
            }
            else if (rowArray[0] == "deck")
            {
                int id = int.Parse(rowArray[1]);
                int num = int.Parse(rowArray[2]);
                m_Deck[id] = num;
            }
        }
    }
    #endregion

    #region Unity回调
    #endregion

    #region 事件回调
    #endregion

    #region 帮助方法
    #endregion

}
