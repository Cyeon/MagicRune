using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEPosionousLiquidEnd : StatusEvent
{
    public override void Invoke()
    {
        base.Invoke();
        _unit.OnTakeDamage.RemoveListener(x => TurnSkip(x));
    }

    public void TurnSkip(float dmg)
    {
        if (dmg > 0)
        {
            _unit.isTurnSkip = true;
        }
    }
}
