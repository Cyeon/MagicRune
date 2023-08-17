using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfGeneration : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Addressable.Load<BaseRuneSO>("SO/Rune/Electric/" + typeof(SelfGeneration).Name);
        base.Init();
    }

    public override void AbilityAction()
    {
        Managers.GetPlayer().StatusManager.AddStatus(StatusName.SelfGeneration, GetAbliltiValue(EffectType.Status, StatusName.SelfGeneration).RoundToInt());
    }

    public override object Clone()
    {
        SelfGeneration selfGeneration = new SelfGeneration();
        selfGeneration.Init();
        selfGeneration.UnEnhance();
        return selfGeneration;
    }
}
