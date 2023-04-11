using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailGun : BaseRune
{
    public override void Init()
    {
        _baseCardSO = Managers.Resource.Load<BaseRuneSO>("SO/Rune/" + typeof(RailGun).Name);
    }

    public override bool AbilityCondition()
    {
        float statusValue = StatusManager.Instance.GetUnitStatusValue(Managers.GetPlayer(), StatusName.Recharging);

        return statusValue >= 10;
    }

    public override void AbilityAction()
    {
        if(AbilityCondition())
        {
            StatusManager.Instance.RemoveValue(Managers.GetPlayer(), StatusName.Recharging, 10);

            Managers.GetPlayer().Attack(GetAbliltiValaue(EffectType.Attack));
        }
    }
}
