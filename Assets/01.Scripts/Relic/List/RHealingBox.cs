using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RHealingBox : Relic
{
    public int healingValue;

    public override void OnAdd()
    {
        Managers.GetPlayer().AddHP(healingValue);
    }

    public override void OnRemove()
    {

    }
}
