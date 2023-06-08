using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundBeat : BaseRune
{
    public override void Init()
    {
        base.Init();
        _baseRuneSO = Managers.Resource.Load<BaseRuneSO>("SO/Rune/Ground/" + typeof(GroundBeat).Name);
    }

    public override void AbilityAction()
    {
        Managers.GetPlayer().StatusManager.AddStatus(StatusName.GroundBeat, GetAbliltiValue(EffectType.Status, StatusName.GroundBeat).RoundToInt());
    }

    public override object Clone()
    {
        GroundBeat groundBeat = new GroundBeat();
        groundBeat.Init();
        groundBeat.UnEnhance();
        return groundBeat;
    }
}
