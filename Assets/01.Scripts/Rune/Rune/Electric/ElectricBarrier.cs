using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBarrier : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Resource.Load<BaseRuneSO>("SO/Rune/Electric/" + typeof(ElectricBarrier).Name);
    }

    public override bool AbilityCondition()
    {
        float statusValue = Managers.GetPlayer().StatusManager.GetStatusValue(StatusName.Recharging);

        return statusValue >= 5;
    }

    public override void AbilityAction()
    {
        Managers.GetPlayer().AddShield(GetAbliltiValue(EffectType.Defence));
        Managers.GetPlayer().StatusManager.RemoveStatus(StatusName.Recharging, (int)GetAbliltiValue(EffectType.DestroyStatus, StatusName.Recharging));
    }

    public override object Clone()
    {
        ElectricBarrier electricBarrier = new ElectricBarrier();
        electricBarrier.Init();
        electricBarrier.UnEnhance();
        return electricBarrier;
    }
}
