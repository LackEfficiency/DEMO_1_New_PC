using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIStart : View
{
    public override string Name
    {
        get { return Consts.V_Start; }
    }

    public void GotoSelect() //�����ť��ת����
    {
        Game.Instance.LoadScene(2);
    }

    public override void HandleEvent(string eventName, object data = null)
    {

    }
}