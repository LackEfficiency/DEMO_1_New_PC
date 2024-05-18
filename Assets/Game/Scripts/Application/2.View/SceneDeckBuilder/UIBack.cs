using UnityEngine;
using UnityEngine.EventSystems;
using System;
using Unity.UI;
using UnityEngine.UI;


public class UIBack : View
{
    public int BackSceneIndex; //返回场景的索引号

    public Button btnBack;

    GameModel m_gameModel;

    public override string Name
    {
        get { return Consts.V_Back; }
    }

    public void OnClickBack()
    {
        //跳转回上个页面
        Game.Instance.LoadScene(BackSceneIndex);
    }

    public override void RegisterEvents()
    {
        AttentionEvents.Add(Consts.E_EnterScene);
    }

    public override void HandleEvent(string eventName, object data = null)
    {
        switch (eventName)
        {
            case Consts.E_EnterScene:
                m_gameModel = GetModel<GameModel>();
                BackSceneIndex = m_gameModel.CurrentSceneIndex;
                break;

            default:
                break;
        }
}
}
