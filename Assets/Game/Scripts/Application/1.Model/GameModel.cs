using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using TMPro.Examples;

public class GameModel : Model //存储游戏数据
{
    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    //所有的关卡
    List<Level> m_Levels = new List<Level>();

    //存储卡牌信息
    //这里不需要使用字典 卡牌的位置索引即为实际的索引
    List<CardInfo> m_Cards = new List<CardInfo>();

    //效果管理器
    EffectManager m_EffectManager;

    //Buff管理器
    BuffManager m_BuffManager;  

    //当前游戏的关卡索引
    int m_PlayLevelIndex = -1;

    //最大通关关卡索引
    int m_GameProgress = -1;

    //游戏当前剩余次数
    int m_Chance = -1;

    //游戏当前血量
    int m_CurrentHP = -1;

    //是否游戏中
    bool m_isPlaying = false;

    //当前场景索引值
    int m_CurrentSceneIndex;
    #endregion

   
    #region 属性
    public override string Name //类名
    {
        get { return Consts.M_GameModel; }
    }

    public List<CardInfo> Cards //所有的卡牌信息
    {
        get { return m_Cards; }
    }

    public int CardCount //卡牌总数
    {
        get { return m_Cards.Count; }
    }

    public List<Level> AllLevels  //所有的关卡
    {
        get { return m_Levels; }
    }

    public Level PlayLevel  //玩家当前游玩的关卡
    {
        get
        {
            if (m_PlayLevelIndex < 0 || m_PlayLevelIndex > m_Levels.Count - 1)
                throw new IndexOutOfRangeException("关卡不存在");
            else
                return m_Levels[m_PlayLevelIndex];
        }
    }

    public int LevelCount //关卡总数
    {
        get { return m_Levels.Count; }
    }

    public int GameProgress //最大通关关卡索引
    {
        get => m_GameProgress;
    }

    public int PlayLevelIndex //玩家当前游玩的关卡索引
    {
        get => m_PlayLevelIndex;
    }

    public bool IsGamePassed //是否通关
    {
        get { return GameProgress >= LevelCount - 1; }
    }

    public int Chance
    {
        get { return m_Chance; }
    }

    public int CurrentHP 
    {
        get { return m_CurrentHP; }
    }

    public bool IsPlaying
    {
        get { return m_isPlaying; }
        set { m_isPlaying = value; }
    }

    public int CurrentSceneIndex 
    { 
        get => m_CurrentSceneIndex; 
        set => m_CurrentSceneIndex = value; 
    }

    public EffectManager EffectManager 
    { 
        get => m_EffectManager; 
        set => m_EffectManager = value; 
    }
    public BuffManager BuffManager 
    {
        get => m_BuffManager; 
        set => m_BuffManager = value; 
    }

    #endregion

    #region 方法
    public void Initialize() //在scene0里调用初始化游戏
    {
        //构建Level集合
        List<FileInfo> files = Tools.GetLevelFiles();
        List<Level> levels = new List<Level>();

        for (int i = 0; i < files.Count; i++)
        {
            Level level = new Level();
            Tools.FillLevel(files[i].FullName, ref level);
            levels.Add(level);
        }

        m_Levels = levels;
        //TODO: 读取游戏进度

        //读取卡牌数据
        Tools.LoadCardData(ref m_Cards);

        //读取效果数据
        m_EffectManager = new EffectManager();
        Tools.LoadEffects(ref m_EffectManager);

        //读取Buff数据
        m_BuffManager = new BuffManager();
        Tools.LoadBuffs(ref m_BuffManager);

    }

    //游戏开始
    public void StartLevel(int levelIndex)
    {
        m_PlayLevelIndex = levelIndex;
        m_isPlaying = true;
    }

    //游戏结束
    public void StopLevel(bool isWin)
    {
        if (isWin && (PlayLevelIndex > GameProgress))
        {
            //保存进度
            Saver.SetMapProgress(PlayLevelIndex);
            //更新内存
            m_GameProgress = Saver.GetMapProgress();

        }
        m_isPlaying = false;

    }

    //清档
    public void ClearProgress()
    {
        m_PlayLevelIndex = -1;
        m_isPlaying = false;
        m_GameProgress = -1;
        m_Chance = -1;
        m_CurrentHP = -1;
        Saver.SetMapProgress(-1);
    }

    //复制卡牌
    //防止引用传递
    //战斗中需要对卡牌的数据进行修改，需要留存一份卡牌的原始数据
    public CardInfo CopyCard(int id)
    {
        CardInfo copyCard = new CardInfo(Cards[id].CardType, Cards[id].CardID, Cards[id].CardName, Cards[id].Cost);
        if (Cards[id].CardType == CardType.Monster)
        {
            var monstercard = Cards[id] as MonsterCardInfo;
            copyCard = new MonsterCardInfo(monstercard.CardType, monstercard.CardID, monstercard.CardName, monstercard.Cost, monstercard.Attack, monstercard.HP);
        }
        else if (Cards[id].CardType == CardType.Spell)
        {
            var spellcard = Cards[id] as SpellCardInfo;
            copyCard = new SpellCardInfo(spellcard.CardType, spellcard.CardID, spellcard.CardName, spellcard.Cost, spellcard.Effect, spellcard.SpellType);
        }
        return copyCard;
    } 


    #endregion

    #region Unity回调
    #endregion

    #region 事件回调
    #endregion

    #region 帮助方法
    #endregion


}
