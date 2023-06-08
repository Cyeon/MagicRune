using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRegeneration : BaseRune
{
    public override void Init()
    {
        base.Init();
        _baseRuneSO = Managers.Resource.Load<BaseRuneSO>("SO/Rune/Fire/" + typeof(FireRegeneration).Name);
    }

    public override void AbilityAction()
    {
        Managers.GetPlayer().AddHP(GetAbliltiValue(EffectType.Etc), true);
        Managers.GetPlayer().StatusManager.AddStatus(StatusName.Fire, GetAbliltiValue(EffectType.Status).RoundToInt());
    }

    public override object Clone()
    {
        FireRegeneration fireRegeneration = new FireRegeneration();
        fireRegeneration.Init();
        fireRegeneration.UnEnhance();
        return fireRegeneration;
    }
}
