using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIBattleCard : View, IPointerDownHandler
{
    #region ����
    #endregion

    #region �¼�
    #endregion

    #region �ֶ�
    public CardStateBattle m_State = CardStateBattle.inHand;
    #endregion

    #region ����
    public override string Name
    {
        get { return Consts.V_BattleCard; }
    }

    #endregion

    #region ����
    #endregion

    #region Unity�ص�
    #endregion

    #region �¼��ص�
    public override void HandleEvent(string eventName, object data = null)
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //�������ʱ�����ٻ��������� 
        UICard uIcard = this.GetComponent<UICard>();
        CardInfo cardInfo = uIcard.CardInfo;
        if (cardInfo.Cost == 0) //���ҽ�������ʣ��ȴ��غ�Ϊ0�� ���ܷ����¼�
        {   
            if (cardInfo.m_CardType == CardType.Monster && m_State == CardStateBattle.inHand) //���޿����ܷ����ٻ��¼� 
            {
                SummonCardRequestArgs e = new SummonCardRequestArgs();
                e.cardInfo = cardInfo;
                e.go = this.gameObject;
                e.player = Player.Self;
                SendEvent(Consts.E_SummonCardRequest, e);
            }
            //TODO:  ħ������ʹ��

        }
        //Debug.Log("���Ƶ��:" + this.GetComponent<UICard>().CardInfo.CardName);
    }
    #endregion

    #region ��������
    #endregion




}
