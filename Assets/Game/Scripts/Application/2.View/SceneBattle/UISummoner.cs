using System.Collections;
using TMPro;
using UnityEngine;

public class UISummoner : View
{
    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    //所有需要动态加载的单位要素
    public TextMeshProUGUI m_Hp;
    public TextMeshProUGUI m_HandCards;
    public TextMeshProUGUI m_RemainingCards;
    private Summoner m_Summoner;
    #endregion

    #region 属性
    public override string Name
    {
        get { return Consts.V_Summoner; }
    }

    public Summoner Summoner { get => m_Summoner; set => m_Summoner = value; }
    #endregion

    #region 方法
    //更新当前信息
    public void Show()
    {
        m_RemainingCards.text = "(" + m_Summoner.RemainingCards.ToString() + ")";
        m_HandCards.text = m_Summoner.HandCards.ToString();
        m_Hp.text = m_Summoner.Hp.ToString();
    }
    #endregion

    #region Unity回调
    #endregion

    #region 事件回调
    public override void RegisterEvents()
    {
        
    }
    #endregion

    #region 帮助方法
    #endregion

    public override void HandleEvent(string eventName, object data = null)
    {
        
    }
}