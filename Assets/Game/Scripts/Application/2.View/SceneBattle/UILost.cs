using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UILost : View
{
    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    public Button btnRestart;
    public Button btnExit;
    #endregion

    #region 属性
    public override string Name
    {
        get { return Consts.V_Lost; }
    }
    #endregion

    #region 方法
    public void Show() //显示界面
    {
        this.gameObject.SetActive(true);
    }

    public void Hide() //隐藏界面
    {
        this.gameObject.SetActive(false);
    }
    #endregion

    #region Unity回调
    #endregion

    #region 事件回调
    public override void HandleEvent(string eventName, object data = null)
    {

    }

    public void OnRestartClick()
    {
        //有bug 以后再修
        Game.Instance.LoadScene(1);

        //GameModel gModel = GetModel<GameModel>();
        //SendEvent(Consts.E_StartLevel, new StartLevelArgs() { LevelID = gModel.PlayLevelIndex });
    }

    public void OnExitClick()
    {
        Game.Instance.LoadScene(1);
    }
    #endregion

    #region 帮助方法
    #endregion
}