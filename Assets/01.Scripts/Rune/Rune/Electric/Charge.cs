using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Resource.Load<BaseRuneSO>("SO/Rune/Electric/" + typeof(Charge).Name);
    }

    public override void AbilityAction()
    {
        Managers.GetPlayer().StatusManager.AddStatus(StatusName.Recharging, GetAbliltiValue(EffectType.Status, StatusName.Recharging).RoundToInt());
    }

    public override object Clone()
    {
        Charge charge = new Charge();
        charge.Init();
        charge.UnEnhance();
        return charge;
    }
}
