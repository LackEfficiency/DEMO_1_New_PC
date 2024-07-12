using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIInformationWindow : View
{
    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    public GameObject scrollView; // UI 面板
    public Transform content; // Scroll View 的 Content 对象，用于显示技能和buff信息
    public ScrollRect scrollRect; // Scroll View 的 ScrollRect 组件

    public TextMeshProUGUI skillText; // 显示技能信息的 Text
    public TextMeshProUGUI buffText; // 显示buff信息的 Text

    private MonsterCard m_MonsterCard; // 当前卡牌
    List<BuffInstance> m_BuffInstances; // 当前卡牌的Buff实例
    List<SkillInstance> m_SkillInstances; // 当前卡牌的技能实例
    #endregion

    #region 属性
    public override string Name
    {
        get { return Consts.V_UIInformationWindow; }
    }

    public MonsterCard MonsterCard { get => m_MonsterCard; set => m_MonsterCard = value; }
    public List<BuffInstance> BuffInstances { get => m_BuffInstances; set => m_BuffInstances = value; }
    public List<SkillInstance> SkillInstances { get => m_SkillInstances; set => m_SkillInstances = value; }
    #endregion

    #region 方法
    private void UpdateUI()
    {
        // 清空当前内容
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        // 更新Skill信息
        SkillInstances = Game.Instance.SkillManager.GetSkillOnCard(MonsterCard);
        if (SkillInstances != null)
        {
            foreach (var skillInstance in SkillInstances)
            {
                TextMeshProUGUI text = Instantiate(skillText, content);
                text.transform.localScale = new Vector3(1f, 1f, 1f);
                text.GetComponent<TextMeshProUGUI>().text = skillInstance.SkillBase.SkillName;
            }
        }

        // 更新Buff信息
        //TOOD: 所有Buff增加图标，且不显示技能buff
        BuffInstances = Game.Instance.BuffManager.GetBuffOnCard(MonsterCard);
        if (BuffInstances != null)
        {
            foreach (var buffInstance in BuffInstances)
            {
                TextMeshProUGUI text = Instantiate(buffText, content);
                text.transform.localScale = new Vector3(1f, 1f, 1f);
                text.GetComponent<TextMeshProUGUI>().text = buffInstance.BuffBase.BuffName + ": " + buffInstance.RemainingRound.ToString();
                
            }
        }
        // TODO: 动态显示/隐藏滚动条

    }

    public void Show(Vector3 pos, MonsterCard monsterCard)
    {
        pos.x += 300;
        scrollView.transform.position = pos;
        MonsterCard = monsterCard;
        monsterCard.Dead -= HideWindow;
        monsterCard.Dead += HideWindow;
        UpdateUI();
        scrollView.SetActive(true);
    }

    public void Hide()
    {
        scrollView.SetActive(false);
    }

    //当单位死亡时鼠标放在窗口上 则窗口不会消失 因此需要处理这种情况 监听单位死亡事件主动关闭窗口
    public void HideWindow(Card monsterCard)
    {
        Hide();
    }

    #endregion

    #region Unity回调
    #endregion

    #region 事件回调
    public override void RegisterEvents()
    {
        this.AttentionEvents.Add(Consts.E_EnterScene);
    }
    public override void HandleEvent(string eventName, object data = null)
    {
        switch (eventName)
        {
            case Consts.E_EnterScene:
                SceneArgs e0 = data as SceneArgs;
                if (e0.SceneIndex == 3)
                {
                    scrollView.SetActive(false);

                }
                break;
        }


    }

    #endregion

    #region 帮助方法
    #endregion
}