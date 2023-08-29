using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncing : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Addressable.Load<BaseRuneSO>("SO/Rune/Ground/" + typeof(Bouncing).Name);
        base.Init();
    }

    public override void AbilityAction()
    {
        Managers.GetPlayer().StatusManager.AddStatus(StatusName.Bouncing, GetAbliltiValue(EffectType.Status, StatusName.Bouncing).RoundToInt());
    }

    public override object Clone()
    {
        Bouncing bouncing = new Bouncing();
        bouncing.Init();
        bouncing.UnEnhance();
        return bouncing;
    }
}
