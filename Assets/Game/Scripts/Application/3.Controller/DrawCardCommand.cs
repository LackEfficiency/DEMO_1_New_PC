using System;
using System.Collections.Generic;

class DrawCardCommand : Controller
{
    public override void Execute(object data = null)
    {
        DrawCardArgs e = data as DrawCardArgs;

        
        RoundModel rModel = GetModel<RoundModel>();
        rModel.DrawCard(e.nums);
    }
}