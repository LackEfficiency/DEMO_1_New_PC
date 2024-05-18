using UnityEngine;
using UnityEngine.EventSystems;
using System;
using Unity.UI;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UICountRound : View
{
    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    public TextMeshProUGUI m_CurrentRound;
    RoundModel rModel;
    int m_CurrentRoundIndex;
    #endregion

    #region 属性
    public override string Name
    {
        get { return Consts.V_CountRound; } 
    }
    #endregion

    #region 方法
    public void Show() //显示界面
    {
        m_CurrentRound.text = (m_CurrentRoundIndex+1).ToString();
        this.gameObject.SetActive(true);
    }

    public void Hide() //隐藏界面
    {
        this.gameObject.SetActive(false);
    }

    public void ShowRound()
    {
        Show();
        StartCoroutine(Display()); //展示回合数
    }

    IEnumerator Display()
    {

        int count = 2; //等待回合显示事件
        while (count > 0)
        {
            //等待一秒
            count--;
            yield return new WaitForSeconds(1f);
       
            if (count <= 0)
                    break;
        }
        Hide(); //倒计时结束
        rModel.m_IsCountDownComplete = true;
    }
    #endregion

    #region Unity回调
    #endregion

    #region 事件回调
    public override void RegisterEvents()
    {
        this.AttentionEvents.Add(Consts.E_StartRound);
        this.AttentionEvents.Add(Consts.E_EnterScene);
    }

    public override void HandleEvent(string eventName, object data = null)
    {
        switch (eventName)
        {
            case Consts.E_StartRound:
                StartRoundArgs e = data as StartRoundArgs;
                m_CurrentRoundIndex = e.RoundIndex;
                ShowRound();
                break;
            case Consts.E_EnterScene:
                rModel = GetModel<RoundModel>();
                rModel.StartRound();
                break;
            default:
                break;
        }
    }
    #endregion

    #region 帮助方法
    #endregion
}