using System;
using System.Collections;
using UnityEngine;

//鼠标点击参数
public class TileSelectClickEventArgs : EventArgs
{
    public int MouseButton; //0是左键，1是右键
    public Room tile;

    public TileSelectClickEventArgs(int mouseButton, Room tile)
    {
        MouseButton = mouseButton;
        this.tile = tile;
    }
}
