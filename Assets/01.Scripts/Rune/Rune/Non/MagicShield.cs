using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicShield : BaseRune
{
    public override bool AbilityCondition()
    {
        return true;
    }

    public override void AbilityAction()
    {
        Managers.GetPlayer().AddShield(GetAbliltiValaue(EffectType.Defence));
    }
}
