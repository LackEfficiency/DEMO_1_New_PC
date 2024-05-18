using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UICard:View
{
    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    //Test Git
    //所有需要动态加载的卡牌要素
    public TextMeshProUGUI m_CardName;
    public TextMeshProUGUI m_Effect;
    public TextMeshProUGUI m_Atk;
    public TextMeshProUGUI m_HP;
    public TextMeshProUGUI m_MaxHP;
    public TextMeshProUGUI m_CostNum;

    public Image m_Bg;
    public Image m_Rim;

    private CardInfo cardInfo;

    public TextMeshProUGUI m_Count;
    private CardState state; //处于卡堆中还是卡组中
    #endregion

    #region 属性
    public override string Name
    {
        get { return Consts.V_Card; }
    }

    public CardState State 
    { 
        get => state; 
        set => state = value;
    }

    public CardInfo CardInfo 
    { 
        get => cardInfo;
        set
        {
            cardInfo = value;
            if (cardInfo != null)
            {
                cardInfo.DataChanged += Show;
                Show(); // 更新UI显示
            }
        }
    }

    #endregion

    #region 方法
    //展示当前卡
    public void Show()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex != 4 ) //如果不在组卡界面，则不显示数量
            m_Count.gameObject.SetActive( false );

        m_CardName.text = CardInfo.CardName;
        if (CardInfo is MonsterCardInfo)
        {
            var monsterInfo = CardInfo as MonsterCardInfo;
            m_Atk.text =  monsterInfo.Attack.ToString();
            m_HP.text = monsterInfo.HP.ToString();
            m_MaxHP.text = "(" + monsterInfo.MaxHP.ToString() + ")";
            m_CostNum.text = monsterInfo.Cost.ToString();
            m_Effect.gameObject.SetActive(false);
        }
        else if (CardInfo is SpellCardInfo)
        {
            var spell = CardInfo as SpellCardInfo;
            m_Effect.text = spell.Effect.ToString();
            m_CostNum.text = spell.Cost.ToString();
            
            m_Atk.gameObject.SetActive(false);
            m_HP.gameObject.SetActive(false);
            m_MaxHP.gameObject.SetActive(false);
        }
    }

    #endregion

    #region Unity回调
    #endregion

    #region 事件回调
    public override void RegisterEvents()
    {
        base.RegisterEvents();
    }
    public override void HandleEvent(string eventName, object data = null)
    {

    }
    #endregion

    #region 帮助方法
    #endregion
}
