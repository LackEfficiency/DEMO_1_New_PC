using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;



public class ClickCard : View, IPointerDownHandler
{
    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    DeckManager m_DeckManager;
    PlayerModel m_PlayerModel;

    public CardState state; //卡牌状态，在卡组还是在卡堆

    private bool canHandleClick = true; //防止重复点击
    private float clickCooldown = 0.5f; // 点击冷却时间，单位为秒
    #endregion

    #region 属性
    public override string Name
    {
        get { return Consts.V_ClickCard; }
    }

    #endregion

    #region 方法
    #endregion

    #region Unity回调
    public void OnPointerDown(PointerEventData eventData) //点击卡牌 移动卡牌至卡堆或卡组
    {
        if (canHandleClick)
        {
            // 处理点击事件
            Card card = this.GetComponent<Card>();
            SendEvent(Consts.E_ClickCard, new ClickCardArgs() { ClickCard = card });

            // 设置点击冷却时间
            canHandleClick = false;
            Invoke("ResetClickCooldown", clickCooldown);
        }   

    }

    private void ResetClickCooldown()
    {
        canHandleClick = true;
    }
    #endregion

    #region 事件回调
    public override void RegisterEvents()
    {
        AttentionEvents.Add(Consts.E_EnterScene);
    }

    public override void HandleEvent(string eventName, object data = null)
    {
        switch (eventName)
        {
            case Consts.E_EnterScene:
                SceneArgs e0 = data as SceneArgs;
                if (e0.SceneIndex == 4)
                {
                    m_DeckManager = GameObject.Find("Canvas").transform.Find("DeckManager").GetComponent<DeckManager>();
                    m_PlayerModel = GetModel<PlayerModel>();
                }
                break;
        }
    }
    #endregion

    #region 帮助方法
    #endregion
}