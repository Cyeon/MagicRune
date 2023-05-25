using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailGun : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Resource.Load<BaseRuneSO>("SO/Rune/Electric/" + typeof(RailGun).Name);
    }

    public override bool AbilityCondition()
    {
        float statusValue = Managers.GetPlayer().StatusManager.GetStatusValue(StatusName.Recharging);

        return statusValue >= 10;
    }

    public override void AbilityAction()
    {
        Managers.GetPlayer().Attack(GetAbliltiValue(EffectType.Attack), IsIncludeKeyword(KeywordType.Penetration));
        Managers.GetPlayer().StatusManager.RemoveStatus(StatusName.Recharging, 10);
    }

    public override object Clone()
    {
        RailGun railGun = new RailGun();
        railGun.Init();
        return railGun;
    }
}
