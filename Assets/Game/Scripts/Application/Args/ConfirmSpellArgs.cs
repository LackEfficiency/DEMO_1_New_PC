using System;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmSpellArgs
{
    //召唤位置
    public TileBattle tile;

    //使用法术的玩家
    public Player player; //0代表自己 1代表对手
}