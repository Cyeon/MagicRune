using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;                 

public class IceShield : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Addressable.Load<BaseRuneSO>("SO/Rune/Ice/" + typeof(IceShield).Name);
        base.Init();
    }
    public override void AbilityAction()
    {
        Managers.GetPlayer().AddShield(GetAbliltiValue(EffectType.Defence));
        Managers.GetPlayer().StatusManager.AddStatus(StatusName.IceShield, GetAbliltiValue(EffectType.Status).RoundToInt());
    }

    public override object Clone()
    {
        IceShield iceShield = new IceShield();
        iceShield.Init();
        iceShield.UnEnhance();
        return iceShield;
    }
}
