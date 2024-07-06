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
    private Card m_Card;
    #endregion

    #region 属性
    public override string Name
    {
        get { return Consts.V_UnitStatus; }
    }

    public Card Card
    {
        get => m_Card;
        set => m_Card = value;
    }

    #endregion

    #region 方法
    //展示当前角色
    public void Show()
    {
        if (Card is MonsterCard)
        {
            var monsterCard = Card as MonsterCard;

            //展示属性
            int totalAttack = Math.Max(0, monsterCard.BaseAttack + monsterCard.AttackBoost);
            m_Atk.text = totalAttack.ToString();

            //根据额外攻击力的正负，显示颜色
            if (monsterCard.AttackBoost > 0)
            {
                m_Atk.color = Color.green;
            }
            else if (monsterCard.AttackBoost < 0)
            {
                m_Atk.color = Color.red;
            }

            m_HP.text = monsterCard.Hp.ToString();

            //根据额外生命值的正负，显示颜色
            int totalHp = Math.Max(0, monsterCard.MaxHp + monsterCard.MaxHpBoost);
            m_MaxHP.text = "(" + totalHp.ToString() + ")";
            //根据额外生命值的正负，显示颜色
            if (monsterCard.MaxHpBoost > 0)
            {
                m_MaxHP.color = Color.green;
            }
            else if (monsterCard.MaxHpBoost < 0)
            {
                m_MaxHP.color = Color.red;
            }

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
