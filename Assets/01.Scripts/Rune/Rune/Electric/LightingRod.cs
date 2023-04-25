using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingRod : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Resource.Load<BaseRuneSO>("SO/Rune/Electric/" + typeof(LightingRod).Name);
    }

    public override void AbilityAction()
    {
        Managers.GetPlayer().StatusManager.AddStatus(StatusName.Recharging, GetAbliltiValaue(EffectType.Status).RoundToInt());
        Managers.GetPlayer().TakeDamage(GetAbliltiValaue(EffectType.Attack));
    }
}
