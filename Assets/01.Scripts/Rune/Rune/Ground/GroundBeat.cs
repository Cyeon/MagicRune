using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundBeat : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Addressable.Load<BaseRuneSO>("SO/Rune/Ground/" + typeof(GroundBeat).Name);
        base.Init();
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
