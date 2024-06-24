using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIStore : View
{
    #region ����
    #endregion

    #region �¼�
    #endregion

    #region �ֶ�

    //����Ϸģ�ʹ��濨����Ϣ 
    GameModel m_GameModel;
    #endregion

    #region ����
    public override string Name
    {
        get { return Consts.V_Store; }
    } 

    public GameModel GameModel
    { 
        get => m_GameModel; 
    }

    #endregion
    #region ����

    public CardInfo RandomCard() //�����һ�ſ�
    {
        CardInfo cardInfo = GameModel.Cards[UnityEngine.Random.Range(0, GameModel.CardCount)];
        return cardInfo;
    }
    #endregion

    #region Unity�ص�
    #endregion

    #region �¼��ص�
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
                if (e0.SceneIndex == 1)
                {
                    m_GameModel = GetModel<GameModel>();
                    PlayerModel pModel = GetModel<PlayerModel>();
                }
                break;
            default: break;
        }
    }
    #endregion

    #region ��������
    #endregion
}
