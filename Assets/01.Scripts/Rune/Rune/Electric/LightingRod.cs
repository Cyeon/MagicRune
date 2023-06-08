using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingRod : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Resource.Load<BaseRuneSO>("SO/Rune/Electric/" + typeof(LightingRod).Name);
        base.Init();
    }

    public override void AbilityAction()
    {
        Managers.GetPlayer().StatusManager.AddStatus(StatusName.Recharging, GetAbliltiValue(EffectType.Status, StatusName.Recharging).RoundToInt());
        Managers.GetPlayer().TakeDamage(GetAbliltiValue(EffectType.Attack));
    }

    public override object Clone()
    {
        LightingRod lightingRod = new LightingRod();
        lightingRod.Init();
        lightingRod.UnEnhance();
        return lightingRod;
    }
}
