using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : BaseRune
{
    public override void AbilityAction()
    {
        Managers.GetPlayer().StatusManager.AddStatus(StatusName.Recharging, 5);
    }
}
