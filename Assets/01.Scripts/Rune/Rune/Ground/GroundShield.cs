using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundShield : BaseRune
{
    public override void AbilityAction()
    {
        Managers.GetPlayer().AddShield(GetAbliltiValaue(EffectType.Defence));
    }
}
