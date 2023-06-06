using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicShield : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Resource.Load<BaseRuneSO>("SO/Rune/Non/" + typeof(MagicShield).Name);
    }

    public override void AbilityAction()
    {
        Managers.GetPlayer().AddShield(GetAbliltiValue(EffectType.Defence));
    }

    public override object Clone()
    {
        MagicShield magicShield = new MagicShield();
        magicShield.Init();
        magicShield.UnEnhance();
        return magicShield;
    }
}
