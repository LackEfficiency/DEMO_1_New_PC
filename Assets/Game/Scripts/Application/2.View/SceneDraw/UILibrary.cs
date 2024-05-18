using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UILibrary : View
{
    public override string Name
    {
        get { return Consts.V_Library; }
    }

    public void GotoSelect() //点击按钮跳转至组卡场景
    {
        //需要带上索引号
        GameModel gm = GetModel<GameModel>();
        gm.CurrentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        Game.Instance.LoadScene(4);
    }

    public override void HandleEvent(string eventName, object data = null)
    {

    }
}