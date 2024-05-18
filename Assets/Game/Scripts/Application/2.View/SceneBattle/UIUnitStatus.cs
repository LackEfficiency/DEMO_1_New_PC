using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIUnitStatus : View
{
    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    //所有需要动态加载的单位要素
    public TextMeshProUGUI m_Atk;
    public TextMeshProUGUI m_HP;
    public TextMeshProUGUI m_MaxHP;
    private CardInfo cardInfo;
    #endregion

    #region 属性
    public override string Name
    {
        get { return Consts.V_UnitStatus; }
    }

    public CardInfo CardInfo
    {
        get => cardInfo;
        set => cardInfo = value;
    }

    #endregion

    #region 方法
    //展示当前角色
    public void Show()
    {
        if (CardInfo is MonsterCardInfo)
        {
            var monsterInfo = CardInfo as MonsterCardInfo;
            m_Atk.text = monsterInfo.Attack.ToString();
            m_HP.text = monsterInfo.HP.ToString();
            m_MaxHP.text = "(" + monsterInfo.MaxHP.ToString() + ")";
        }
    }

    #endregion

    #region Unity回调
    public override void RegisterEvents()
    {

    }
    #endregion

    #region 事件回调
    public override void HandleEvent(string eventName, object data = null)
    {

    }
    #endregion

    #region 帮助方法
    #endregion
}
