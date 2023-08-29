using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricAbsorption : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Addressable.Load<BaseRuneSO>("SO/Rune/Electric/" + typeof(ElectricAbsorption).Name);
        base.Init();
    }

    public override void AbilityAction()
    {
        Managers.GetPlayer().StatusManager.AddStatus(StatusName.Recharging, GetAbliltiValue(EffectType.Status, StatusName.Recharging).RoundToInt());
        Managers.GetPlayer().Attack(GetAbliltiValue(EffectType.Attack), IsIncludeKeyword(KeywordName.Penetration));
    }

    public override object Clone()
    {
        ElectricAbsorption electricAbsorption = new ElectricAbsorption();
        electricAbsorption.Init();
        electricAbsorption.UnEnhance();
        return electricAbsorption;
    }
}
