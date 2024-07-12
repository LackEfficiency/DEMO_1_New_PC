 using System;
using System.Collections.Generic;

class ExitSceneCommand : Controller
{
    public override void Execute(object data = null)
    {
        //离开场景前回收所有可回收对象
        Game.Instance.ObjectPool.UnspawnAll();
        
    }
}