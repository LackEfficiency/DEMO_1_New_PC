using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class TurnEndCommand : Controller
{
    public override void Execute(object data = null)
    {
        RoundModel rModel = GetModel<RoundModel>();
        rModel.IsTurnRun = false;
    }
}



