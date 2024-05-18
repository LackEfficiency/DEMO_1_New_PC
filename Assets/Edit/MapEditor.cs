using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(MapBattle))]
public class MapEditor : Editor
{
    [HideInInspector]
    public MapBattle Map = null;

    //�ؿ��б�
    List<FileInfo> m_files = new List<FileInfo>();

    //��ǰ�༭�Ĺؿ�������
    int m_selectIndex = 0;


    public override void OnInspectorGUI()
    {
        //Ĭ�ϻ���
        base.OnInspectorGUI();

        if (Application.isPlaying) //��������Ϸ����ʾѡ��
        {
            //������Mono�ű����
            Map = target as MapBattle;

            //�Զ��������һ��
            EditorGUILayout.BeginHorizontal();
            int currentindex = EditorGUILayout.Popup(m_selectIndex, GetNames(m_files));
            if (currentindex != m_selectIndex)
            {
                m_selectIndex = currentindex;
                //���ص�ǰ��ѡ��ؿ�
                LoadLevel();
            }
            if (GUILayout.Button("��ȡ�б�"))
            {
                //��ȡ�ؿ��б�
                LoadLevelFiles();
            }
            EditorGUILayout.EndHorizontal();

            //���Ƶڶ���
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("����ɽ����"))
            {
                Map.ClearHolder();
            }
            if (GUILayout.Button("����ɷ��õ�"))
            {
                Map.ClearSet();
            }
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("��������"))
            {
                //����ؿ�
                SaveLevel();
            }
        }

        //������Բ����Ķ�����ʶmap����Ϊdirty
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private string[] GetNames(List<FileInfo> files)
    {
        List<string> names = new List<string>();
        foreach (FileInfo file in files)
        {
            names.Add(file.Name);
        }
        return names.ToArray();
    }

    //��ȡ�ؿ��б�
    private void LoadLevelFiles()
    {
        //�����ǰ״̬�� ��ֹ��ͻ
        Clear();

        //�����б�
        m_files = Tools.GetLevelFiles();

        //Ĭ�ϼ��ص�һ���ؿ�
        if (m_files.Count > 0)
        {
            m_selectIndex = 0;
            LoadLevel();
        }
    }

    //����ؿ�
    private void SaveLevel()
    {
        //��ȡ��ǰ���صĹؿ�
        Level level = Map.Level;

        //�ռ��ɽ����
        List<Point> list;

        list = new List<Point>();
        for (int i = 0; i < Map.Grid.Count; i++)
        {
            TileBattle t = Map.Grid[i];
            if (t.CanHold)
            {
                Point p = new Point(t.X, t.Y);
                list.Add(p);
            }
        }
        level.Holder = list;

        //�ռ��ɷ��õ�
        list = new List<Point>();
        for (int i = 0; i < Map.Grid.Count; i++)
        {
            TileBattle t = Map.Grid[i];
            if(t.CanSetEnemy)
            {
                Point p = new Point(t.X, t.Y);
                list.Add(p);
            }
        }
        level.Set = list;

        //����ؿ�
        string fileName = m_files[m_selectIndex].FullName;
        Tools.SaveLevel(fileName, level);

        //������ʾ
        EditorUtility.DisplayDialog("����ؿ�����", "����ɹ�", "ȷ��");
    }

    //���عؿ�
    private void LoadLevel()
    {
        FileInfo file = m_files[m_selectIndex];
        Level level = new Level();
        Tools.FillLevel(file.FullName, ref level);

        Map.LoadLevel(level);
    }

    //�����ǰ��Ϣ
    private void Clear()
    {
        m_files.Clear();
        m_selectIndex = 0;
    }
}

