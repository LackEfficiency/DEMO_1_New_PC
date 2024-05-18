 using System;
using System.Collections.Generic;

class ExitSceneCommand : Controller
{
    public override void Execute(object data = null)
    {
        //离开场景前回收所有可回收对象
        Game.Instance.ObjectPool.UnspawnAll();

        SceneArgs e = data as SceneArgs;
        
        if(e.SceneIndex == 4 || e.SceneIndex == 1) //由于移动了对象池的对象父物体，需要销毁对象池
        {
            Game.Instance.ObjectPool.ClearAll();
        }
        
    }
}