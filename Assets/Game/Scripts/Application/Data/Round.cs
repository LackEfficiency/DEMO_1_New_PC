using System;
using System.Collections.Generic;
using UnityEngine;

public class Round //传入敌人卡牌id list
{
    private List<int> m_EnemyID = new List<int>(); //怪物类型ID

    public List<int> EnemyID 
    { 
        get => m_EnemyID;
    }
}