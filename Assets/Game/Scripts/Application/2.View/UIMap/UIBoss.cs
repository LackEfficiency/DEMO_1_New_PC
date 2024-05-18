using System;
using System.Collections;
using UnityEngine;

public class UIBoss : View
{
    public override string Name
    {
        get{ return Consts.V_Boss; }
    }

    public override void HandleEvent(string eventName, object data = null)
    {

    }
}

