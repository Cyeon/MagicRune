using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailGun : BaseRune
{
    public override bool AbilityCondition()
    {
        float statusValue = StatusManager.Instance.GetUnitStatusValue(Managers.GetPlayer(), StatusName.Recharging);

        return statusValue >= 10;
    }

    public override void AbilityAction()
    {
        StatusManager.Instance.RemoveValue(Managers.GetPlayer(), StatusName.Recharging, 10);

        Managers.GetPlayer().Attack(GetAbliltiValaue(EffectType.Attack));
    }
}
