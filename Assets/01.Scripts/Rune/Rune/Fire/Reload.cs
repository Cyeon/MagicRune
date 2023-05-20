using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reload : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Resource.Load<BaseRuneSO>("SO/Rune/Fire/" + typeof(Reload).Name);
    }

    public override void AbilityAction()
    {
        Managers.GetPlayer().AddHP(GetAbliltiValaue(EffectType.Etc));
        Managers.GetPlayer().StatusManager.AddStatus(StatusName.Fire, GetAbliltiValaue(EffectType.Status).RoundToInt());
    }

    public override object Clone()
    {
        FireRegeneration fireRegeneration = new FireRegeneration();
        fireRegeneration.Init();
        return fireRegeneration;
    }
}
