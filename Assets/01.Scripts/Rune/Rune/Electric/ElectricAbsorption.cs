using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricAbsorption : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Resource.Load<BaseRuneSO>("SO/Rune/Electric/" + typeof(ElectricAbsorption).Name);
    }

    public override void AbilityAction()
    {
        Managers.GetPlayer().StatusManager.AddStatus(StatusName.Recharging, GetAbliltiValue(EffectType.Status).RoundToInt());
        Managers.GetPlayer().Attack(GetAbliltiValue(EffectType.Attack), IsIncludeKeyword(KeywordType.Penetration));
    }

    public override object Clone()
    {
        ElectricAbsorption electricAbsorption = new ElectricAbsorption();
        electricAbsorption.Init();
        return electricAbsorption;
    }
}
