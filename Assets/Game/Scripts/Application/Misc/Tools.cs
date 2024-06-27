using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;

public class Tools
{
    //读取关卡列表
    public static List<FileInfo> GetLevelFiles()
    {
        string[] files = Directory.GetFiles(Consts.LevelDir, "*.xml"); //根据指定目录读取对应后缀的文件存入集合

        List<FileInfo> list = new List<FileInfo>(); //创建一个新的列表存储文件信息
        for (int i = 0; i < files.Length; i++)
        {
            FileInfo file = new FileInfo(files[i]);
            list.Add(file);
        }
        return list;
    }

    //填充Level数据
    public static void FillLevel(string fileName, ref Level level)
    {
        FileInfo file = new FileInfo(fileName);

        StreamReader sr = new StreamReader(file.OpenRead(), Encoding.UTF8);

        XmlDocument doc = new XmlDocument();
        doc.Load(sr);

        level.Name = doc.SelectSingleNode("/Level/Name").InnerText;
        level.LevelIcon = doc.SelectSingleNode("/Level/LevelIcon").InnerText;
        level.Background = doc.SelectSingleNode("/Level/Background").InnerText;
        level.Road = doc.SelectSingleNode("/Level/Road").InnerText;
      
        XmlNodeList nodes;
        XmlNodeList innerNodes;

        nodes = doc.SelectNodes("/Level/Holder/Point"); //填充Holder数据
        for (int i = 0; i < nodes.Count; i++)
        {
            XmlNode node = nodes[i];
            Point p = new Point(
                int.Parse(node.Attributes["X"].Value),
                int.Parse(node.Attributes["Y"].Value));

            level.Holder.Add(p);
        }

        nodes = doc.SelectNodes("/Level/Set/Point"); //填充Set数据，只填充敌人，自己是固定的
        for (int i = 0; i < nodes.Count; i++)
        {
            XmlNode node = nodes[i];
            Point p = new Point(
                int.Parse(node.Attributes["X"].Value),
                int.Parse(node.Attributes["Y"].Value));

            level.Set.Add(p);
        }

        nodes = doc.SelectNodes("/Level/Rounds/Round");//填充Rounds数据
        for (int i = 0; i < nodes.Count; i++)
        {
            Round round = new Round();
            innerNodes = nodes[i].SelectNodes("Enemy"); //填充Enemy数据 
            for (int j = 0; j < innerNodes.Count; j++)
            {
                XmlNode node = innerNodes[j];
                round.EnemyID.Add(int.Parse(node.Attributes["ID"].Value));
            }
            level.Rounds.Add(round);
        }

        sr.Close();
        sr.Dispose();
    }
    
    //保存关卡数据
    public static void SaveLevel(string filename, Level level)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
        sb.AppendLine("<Level>");

        sb.AppendLine(string.Format("<Name>{0}</Name>", level.Name));
        sb.AppendLine(string.Format("<LevelIcon>{0}</LevelIcon>", level.Background));
        sb.AppendLine(string.Format("<Background>{0}</Background>", level.Background));
        sb.AppendLine(string.Format("<Road>{0}</Road>", level.Road));

        sb.AppendLine("<Holder>"); //填充可进入数据
        for (int i = 0; i < level.Holder.Count; i++)
        {
            sb.AppendLine(string.Format("<Point X=\"{0}\" Y=\"{1}\"/>", level.Holder[i].X, level.Holder[i].Y));
        }
        sb.AppendLine("</Holder>");

        sb.AppendLine("<Set>"); //填充可放置数据
        for (int i = 0; i < level.Set.Count; i++)
        {
            sb.AppendLine(string.Format("<Point X=\"{0}\" Y=\"{1}\"/>", level.Set[i].X, level.Set[i].Y));
        }
        sb.AppendLine("</Set>");

        sb.AppendLine("<Rounds>"); //填充回合信息数据
        for (int i = 0; i < level.Rounds.Count; i++)
        {
            sb.AppendLine("<Round>");
            for (int j = 0; j < level.Rounds[i].EnemyID.Count; j++) 
            { 
                sb.AppendLine(string.Format("<Enemy ID=\"{0}\"/>", level.Rounds[i].EnemyID[j])); 
            }         
            sb.AppendLine("</Round>");       
        }
        sb.AppendLine("</Rounds>");

        sb.AppendLine("</Level>");

        string content = sb.ToString();

        StreamWriter sw = new StreamWriter(filename, false, Encoding.UTF8);
        sw.Write(content);
        sw.Flush();
        sw.Dispose();
    }

    //读取卡牌数据
    public static void LoadCardData(ref List<CardInfo> Cards)
    {
        //读取文件
        TextAsset m_CardData = Resources.Load<TextAsset>(Consts.CardDataDir + "/CardList");

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
                string skills = rowArray[6].Replace("\r", "");
                MonsterCardInfo monsterCard = new MonsterCardInfo(CardType.Monster, id, name, cost, atk, hp, skills);

                Cards.Add(monsterCard);
            }
            else if (rowArray[0] == "spell")
            {
                //新建魔法卡
                int id = int.Parse(rowArray[1]);
                string name = rowArray[2];
                string effect = rowArray[3];
                SpellType spellType = (SpellType)Enum.Parse(typeof(SpellType), rowArray[4]);
                int cost = int.Parse(rowArray[5]);
                SpellCardInfo spellCard = new SpellCardInfo(CardType.Spell, id, name, cost, effect, spellType);

                Cards.Add(spellCard);
            }
        }
    }

    //加载图片
    public static IEnumerator LoadImage(string url, SpriteRenderer render)
    {

        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        while (!request.isDone)
        {
            yield return null;
            Debug.Log("错误提示");
        }

        Texture2D texture = DownloadHandlerTexture.GetContent(request);
        Sprite sp = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f));
        render.sprite = sp;
    }

    public static IEnumerator LoadImage(string url, Image image)
    {
        Debug.Log(url);
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        while (!request.isDone)
        {
            yield return null;
            Debug.Log("错误提示");
        }

        Texture2D texture = DownloadHandlerTexture.GetContent(request);
        Sprite sp = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f));
        image.sprite = sp;
    }

    //生成指定长的数列
    public static List<int> GenerateSequence(int length)
    {
        List<int> sequence = new List<int>();
        for (int i = 0; i < length; i++)
        {
            sequence.Add(i);
        }
        return sequence;
    }

    //洗牌数列算法
    public static void ShuffleList<T>(List<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

}
