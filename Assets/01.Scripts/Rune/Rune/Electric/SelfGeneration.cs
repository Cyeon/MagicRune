using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfGeneration : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Resource.Load<BaseRuneSO>("SO/Rune/Electric/" + typeof(SelfGeneration).Name);
    }

    public override void AbilityAction()
    {
        Managers.GetPlayer().StatusManager.AddStatus(StatusName.SelfGeneration, GetAbliltiValue(EffectType.Status).RoundToInt());
    }

    public override object Clone()
    {
        SelfGeneration selfGeneration = new SelfGeneration();
        selfGeneration.Init();
        return selfGeneration;
    }
}
