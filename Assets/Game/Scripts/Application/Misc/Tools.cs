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
    //��ȡ�ؿ��б�
    public static List<FileInfo> GetLevelFiles()
    {
        string[] files = Directory.GetFiles(Consts.LevelDir, "*.xml"); //����ָ��Ŀ¼��ȡ��Ӧ��׺���ļ����뼯��

        List<FileInfo> list = new List<FileInfo>(); //����һ���µ��б�洢�ļ���Ϣ
        for (int i = 0; i < files.Length; i++)
        {
            FileInfo file = new FileInfo(files[i]);
            list.Add(file);
        }
        return list;
    }

    //���Level����
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

        nodes = doc.SelectNodes("/Level/Holder/Point"); //���Holder����
        for (int i = 0; i < nodes.Count; i++)
        {
            XmlNode node = nodes[i];
            Point p = new Point(
                int.Parse(node.Attributes["X"].Value),
                int.Parse(node.Attributes["Y"].Value));

            level.Holder.Add(p);
        }

        nodes = doc.SelectNodes("/Level/Set/Point"); //���Set���ݣ�ֻ�����ˣ��Լ��ǹ̶���
        for (int i = 0; i < nodes.Count; i++)
        {
            XmlNode node = nodes[i];
            Point p = new Point(
                int.Parse(node.Attributes["X"].Value),
                int.Parse(node.Attributes["Y"].Value));

            level.Set.Add(p);
        }

        nodes = doc.SelectNodes("/Level/Rounds/Round");//���Rounds����
        for (int i = 0; i < nodes.Count; i++)
        {
            Round round = new Round();
            innerNodes = nodes[i].SelectNodes("Enemy"); //���Enemy���� 
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
    
    //����ؿ�����
    public static void SaveLevel(string filename, Level level)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
        sb.AppendLine("<Level>");

        sb.AppendLine(string.Format("<Name>{0}</Name>", level.Name));
        sb.AppendLine(string.Format("<LevelIcon>{0}</LevelIcon>", level.Background));
        sb.AppendLine(string.Format("<Background>{0}</Background>", level.Background));
        sb.AppendLine(string.Format("<Road>{0}</Road>", level.Road));

        sb.AppendLine("<Holder>"); //���ɽ�������
        for (int i = 0; i < level.Holder.Count; i++)
        {
            sb.AppendLine(string.Format("<Point X=\"{0}\" Y=\"{1}\"/>", level.Holder[i].X, level.Holder[i].Y));
        }
        sb.AppendLine("</Holder>");

        sb.AppendLine("<Set>"); //���ɷ�������
        for (int i = 0; i < level.Set.Count; i++)
        {
            sb.AppendLine(string.Format("<Point X=\"{0}\" Y=\"{1}\"/>", level.Set[i].X, level.Set[i].Y));
        }
        sb.AppendLine("</Set>");

        sb.AppendLine("<Rounds>"); //���غ���Ϣ����
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

    //����ͼƬ
    public static IEnumerator LoadImage(string url, SpriteRenderer render)
    {

        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        while (!request.isDone)
        {
            yield return null;
            Debug.Log("������ʾ");
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
            Debug.Log("������ʾ");
        }

        Texture2D texture = DownloadHandlerTexture.GetContent(request);
        Sprite sp = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f));
        image.sprite = sp;
    }

    //����ָ����������
    public static List<int> GenerateSequence(int length)
    {
        List<int> sequence = new List<int>();
        for (int i = 0; i < length; i++)
        {
            sequence.Add(i);
        }
        return sequence;
    }

    //ϴ�������㷨
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
