using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEPoisonousLiquid : StatusEvent
{
    public override void Invoke()
    {
        _unit.OnTakeDamage.AddListener(x => TurnSkip(x));
    }

    public void TurnSkip(float dmg)
    {
        if(dmg >0)
        {
            _unit.isTurnSkip = true;
        }
    }
}
