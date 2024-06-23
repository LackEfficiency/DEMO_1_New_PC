using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//�����Ƶĵ���¼�
public class UIBattleCard : View, IPointerDownHandler
{
    #region ����
    #endregion

    #region �¼�
    #endregion

    #region �ֶ�
    public CardStateBattle m_State = CardStateBattle.inHand;
    RoundModel m_rModel = null;
    #endregion

    #region ����
    public override string Name
    {
        get { return Consts.V_BattleCard; }
    }

    public RoundModel rModel { get => m_rModel; set => m_rModel = value; }

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
        rModel = GetModel<RoundModel>();
        //ֻ��������ж��׶β����ٻ���ʹ�ÿ���
        if (rModel.GamePhase != GamePhase.PlayerAct)
        {
            return;
        }

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
            else if (cardInfo.m_CardType == CardType.Spell && m_State == CardStateBattle.inHand) //���������ܷ���ʹ���¼�
            {
                UseSpellCardRequestArgs e = new UseSpellCardRequestArgs();
                e.cardInfo = cardInfo;
                e.go = this.gameObject;
                e.player = Player.Self;
                SendEvent(Consts.E_UseSpellCardRequest, e);
            }

        }
        //Debug.Log("���Ƶ��:" + this.GetComponent<UICard>().MonsterCardInfo.CardName);
    }
    #endregion

    #region ��������
    #endregion




}
