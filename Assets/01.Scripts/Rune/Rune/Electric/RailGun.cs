using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailGun : BaseRune
{
    public override bool AbilityCondition()
    {
        float statusValue = Managers.GetPlayer().StatusManager.GetStatusValue(StatusName.Recharging);

        return statusValue >= 10;
    }

    public override void AbilityAction()
    {
        Managers.GetPlayer().StatusManager.RemoveStatus(StatusName.Recharging, 10);

        Managers.GetPlayer().Attack(GetAbliltiValaue(EffectType.Attack));
    }
}
