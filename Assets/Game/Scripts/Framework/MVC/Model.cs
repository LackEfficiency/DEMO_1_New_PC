using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Model
{
    public abstract string Name { get; }

    protected void SendEvent(string EventName, object data = null)
    {
        MVC.SendEvent(EventName, data);
    }
}