using System;
using System.Collections;
using UnityEngine;

//鼠标点击参数
public class TileBattleClickEventArgs: EventArgs
{
    public int MouseButton; //0是左键，1是右键
    public TileBattle tile;
    public TileBattleClickEventArgs(int mouseButton, TileBattle tile)
    {
        MouseButton = mouseButton;
        this.tile = tile;
    }
}
