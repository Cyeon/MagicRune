using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomDecision : PatternDecision
{
    public override bool MakeADecision()
    {
        return Managers.Enemy.CurrentEnemy.StatusManager.GetStatusValue(StatusName.Boom) == 1;
    }

}
