using System.Collections;
using System;
using UnityEngine;
using Unity.UI;
using TMPro;
using UnityEngine.UIElements;

public class UIShowRound : View
{

    public TextMeshProUGUI m_CurrentRound;

    public override string Name
    {
        get { return Consts.V_ShowRound; }
    }

    public override void RegisterEvents()
    {
        this.AttentionEvents.Add(Consts.E_StartRound);
    }

    public override void HandleEvent(string eventName, object data = null)
    {
        switch (eventName)
        {
            case Consts.E_StartRound:
                StartRoundArgs e = data as StartRoundArgs;
                m_CurrentRound.text = (1+e.RoundIndex).ToString();
                break;
            default:
                break;
        }
    }
}