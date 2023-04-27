using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundShield : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Resource.Load<BaseRuneSO>("SO/Rune/Ground/" + typeof(GroundShield).Name);
    }
    public override void AbilityAction()
    {
        Managers.GetPlayer().AddShield(GetAbliltiValaue(EffectType.Defence));
    }

    public override object Clone()
    {
        GroundShield groundShield = new GroundShield();
        groundShield.Init();
        return groundShield;
    }
}
