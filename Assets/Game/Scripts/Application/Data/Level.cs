using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Level
{
    //名字
    public string Name;

    //关卡图标
    public string LevelIcon;

    //背景
    public string Background;

    //路径背景
    public string Road;

    //可放置的单位位置
    public List<Point> Set = new List<Point>();

    //单位可进入的位置
    public List<Point> Holder = new List<Point>();

    //固定单位回合信息 （若为随机抽取，则动态生成）
    public List<Round> Rounds = new List<Round>();

}