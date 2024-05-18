using System;
using System.Collections.Generic;
using UnityEngine;

static class Saver
{
    static public int GetMapProgress()
    {
        return PlayerPrefs.GetInt(Consts.GameProgress, -1); //从本地读取游戏存档，若不存在则返回-1
    }

    static public void SetMapProgress(int levelIndex)
    {
        PlayerPrefs.SetInt(Consts.GameProgress, levelIndex);
    }
}
