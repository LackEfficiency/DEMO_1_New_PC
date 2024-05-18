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

    public void GotoSelect() //点击按钮跳转场景
    {
        Game.Instance.LoadScene(2);
    }

    public override void HandleEvent(string eventName, object data = null)
    {

    }
}