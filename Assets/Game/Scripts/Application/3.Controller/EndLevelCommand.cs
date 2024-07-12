using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class EndLevelCommand : Controller
{
    public override void Execute(object data = null)
    {
        EndLevelArgs e = data as EndLevelArgs;

        //保存游戏状态
        GameModel gm = GetModel<GameModel>();
        gm.StopLevel(e.IsWin);

        //清除RoundModel数据
        RoundModel rModel = GetModel<RoundModel>();
        rModel.Clear();

        //弹出UI
        if (e.IsWin)
        {
            GetView<UIWin>().Show();
        }
        else
        {
            GetView<UILost>().Show();
        }
    }
}