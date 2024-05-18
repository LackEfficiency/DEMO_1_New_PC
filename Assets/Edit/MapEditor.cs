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

    //关卡列表
    List<FileInfo> m_files = new List<FileInfo>();

    //当前编辑的关卡索引号
    int m_selectIndex = 0;


    public override void OnInspectorGUI()
    {
        //默认绘制
        base.OnInspectorGUI();

        if (Application.isPlaying) //当启动游戏才显示选项
        {
            //关联的Mono脚本组件
            Map = target as MapBattle;

            //自定义绘制在一行
            EditorGUILayout.BeginHorizontal();
            int currentindex = EditorGUILayout.Popup(m_selectIndex, GetNames(m_files));
            if (currentindex != m_selectIndex)
            {
                m_selectIndex = currentindex;
                //加载当前所选择关卡
                LoadLevel();
            }
            if (GUILayout.Button("读取列表"))
            {
                //读取关卡列表
                LoadLevelFiles();
            }
            EditorGUILayout.EndHorizontal();

            //绘制第二行
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("清除可进入点"))
            {
                Map.ClearHolder();
            }
            if (GUILayout.Button("清除可放置点"))
            {
                Map.ClearSet();
            }
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("保存数据"))
            {
                //保存关卡
                SaveLevel();
            }
        }

        //如果属性参数改动，标识map对象为dirty
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

    //读取关卡列表
    private void LoadLevelFiles()
    {
        //清除当前状态， 防止冲突
        Clear();

        //加载列表
        m_files = Tools.GetLevelFiles();

        //默认加载第一个关卡
        if (m_files.Count > 0)
        {
            m_selectIndex = 0;
            LoadLevel();
        }
    }

    //保存关卡
    private void SaveLevel()
    {
        //获取当前加载的关卡
        Level level = Map.Level;

        //收集可进入点
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

        //收集可放置点
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

        //保存关卡
        string fileName = m_files[m_selectIndex].FullName;
        Tools.SaveLevel(fileName, level);

        //弹框提示
        EditorUtility.DisplayDialog("保存关卡数据", "保存成功", "确定");
    }

    //加载关卡
    private void LoadLevel()
    {
        FileInfo file = m_files[m_selectIndex];
        Level level = new Level();
        Tools.FillLevel(file.FullName, ref level);

        Map.LoadLevel(level);
    }

    //清除当前信息
    private void Clear()
    {
        m_files.Clear();
        m_selectIndex = 0;
    }
}

