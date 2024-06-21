using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using TMPro.Examples;

public class GameModel : Model //�洢��Ϸ����
{
    #region ����
    #endregion

    #region �¼�
    #endregion

    #region �ֶ�
    //���еĹؿ�
    List<Level> m_Levels = new List<Level>();

    //�洢������Ϣ
    //���ﲻ��Ҫʹ���ֵ� ���Ƶ�λ��������Ϊʵ�ʵ�����
    List<CardInfo> m_Cards = new List<CardInfo>();

    //��ǰ��Ϸ�Ĺؿ�����
    int m_PlayLevelIndex = -1;

    //���ͨ�عؿ�����
    int m_GameProgress = -1;

    //��Ϸ��ǰʣ�����
    int m_Chance = -1;

    //��Ϸ��ǰѪ��
    int m_CurrentHP = -1;

    //�Ƿ���Ϸ��
    bool m_isPlaying = false;

    //��ǰ��������ֵ
    int m_CurrentSceneIndex;
    #endregion

   
    #region ����
    public override string Name //����
    {
        get { return Consts.M_GameModel; }
    }

    public List<CardInfo> Cards //���еĿ�����Ϣ
    {
        get { return m_Cards; }
    }

    public int CardCount //��������
    {
        get { return m_Cards.Count; }
    }

    public List<Level> AllLevels  //���еĹؿ�
    {
        get { return m_Levels; }
    }

    public Level PlayLevel  //��ҵ�ǰ����Ĺؿ�
    {
        get
        {
            if (m_PlayLevelIndex < 0 || m_PlayLevelIndex > m_Levels.Count - 1)
                throw new IndexOutOfRangeException("�ؿ�������");
            else
                return m_Levels[m_PlayLevelIndex];
        }
    }

    public int LevelCount //�ؿ�����
    {
        get { return m_Levels.Count; }
    }

    public int GameProgress //���ͨ�عؿ�����
    {
        get => m_GameProgress;
    }

    public int PlayLevelIndex //��ҵ�ǰ����Ĺؿ�����
    {
        get => m_PlayLevelIndex;
    }

    public bool IsGamePassed //�Ƿ�ͨ��
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



    #endregion

    #region ����
    public void Initialize() //��scene0����ó�ʼ����Ϸ
    {
        //����Level����
        List<FileInfo> files = Tools.GetLevelFiles();
        List<Level> levels = new List<Level>();

        for (int i = 0; i < files.Count; i++)
        {
            Level level = new Level();
            Tools.FillLevel(files[i].FullName, ref level);
            levels.Add(level);
        }

        m_Levels = levels;
        //TODO: ��ȡ��Ϸ����

    }

    //��Ϸ��ʼ
    public void StartLevel(int levelIndex)
    {
        m_PlayLevelIndex = levelIndex;
        m_isPlaying = true;
    }

    //��Ϸ����
    public void StopLevel(bool isWin)
    {
        if (isWin && (PlayLevelIndex > GameProgress))
        {
            //�������
            Saver.SetMapProgress(PlayLevelIndex);
            //�����ڴ�
            m_GameProgress = Saver.GetMapProgress();

        }
        m_isPlaying = false;

    }

    //�嵵
    public void ClearProgress()
    {
        m_PlayLevelIndex = -1;
        m_isPlaying = false;
        m_GameProgress = -1;
        m_Chance = -1;
        m_CurrentHP = -1;
        Saver.SetMapProgress(-1);
    }

    //���ƿ���
    //��ֹ���ô���
    //ս������Ҫ�Կ��Ƶ����ݽ����޸ģ���Ҫ����һ�ݿ��Ƶ�ԭʼ����
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
            copyCard = new SpellCardInfo(spellcard.CardType, spellcard.CardID, spellcard.CardName, spellcard.Cost, spellcard.Effect);
        }
        return copyCard;
    } 
    #endregion

    #region Unity�ص�
    #endregion

    #region �¼��ص�
    #endregion

    #region ��������
    #endregion


}
