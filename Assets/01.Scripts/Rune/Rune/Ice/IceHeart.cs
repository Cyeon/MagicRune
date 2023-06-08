using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceHeart : BaseRune
{
    public override void Init()
    {
        base.Init();
        _baseRuneSO = Managers.Resource.Load<BaseRuneSO>("SO/Rune/Ice/" + typeof(IceHeart).Name);
    }
    public override void AbilityAction()
    {
        Managers.GetPlayer().StatusManager.AddStatus(StatusName.Chilliness, GetAbliltiValue(EffectType.Status).RoundToInt());
        Managers.GetPlayer().AddShield(GetAbliltiValue(EffectType.Defence));
    }

    public override object Clone()
    {
        IceHeart rune = new IceHeart();
        rune.Init();
        rune.UnEnhance();
        return rune;
    }
}
