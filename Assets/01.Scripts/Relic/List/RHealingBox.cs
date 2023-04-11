using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RHealingBox : Relic, IUseHandler
{
    public int healingValue;

    public void Use()
    {
        Managers.GetPlayer().AddHP(healingValue);
    }
}
