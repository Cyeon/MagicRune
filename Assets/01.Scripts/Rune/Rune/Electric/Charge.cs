using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : BaseRune
{
    public override bool AbilityCondition()
    {
        return true;
    }

    public override void AbilityAction()
    {
        StatusManager.Instance.AddStatus(Managers.GetPlayer(), StatusName.Recharging, 5);
    }
}
