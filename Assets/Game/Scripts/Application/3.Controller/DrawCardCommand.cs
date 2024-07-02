using System;
using System.Collections.Generic;

class DrawCardCommand : Controller
{
    public override void Execute(object data = null)
    {
        DrawCardArgs e = data as DrawCardArgs;

        //更新数据
        RoundModel rModel = GetModel<RoundModel>();
        rModel.DrawCard(e.nums);

        //更新玩家显示
        Spawner spawner = GetView<Spawner>();
        rModel.SelfSummoner.HandCards += e.nums;
        rModel.SelfSummoner.RemainingCards -= e.nums;
        spawner.UpdateSummoner(rModel.SelfSummoner);
    }
}