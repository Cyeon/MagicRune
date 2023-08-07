using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundShield : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Addressable.Load<BaseRuneSO>("SO/Rune/Ground/" + typeof(GroundShield).Name);
        base.Init();
    }
    public override void AbilityAction()
    {
        Managers.GetPlayer().AddShield(GetAbliltiValue(EffectType.Defence));
    }

    public override object Clone()
    {
        GroundShield groundShield = new GroundShield();
        groundShield.Init();
        groundShield.UnEnhance();
        return groundShield;
    }
}
