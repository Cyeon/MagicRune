using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsorptionChilliness : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Resource.Load<BaseRuneSO>("SO/Rune/Ice/" + typeof(AbsorptionChilliness).Name);
        base.Init();
    }
    public override void AbilityAction()
    {
        int cnt = BattleManager.Instance.Enemy.StatusManager.GetStatusValue(StatusName.Chilliness);
        BattleManager.Instance.Enemy.StatusManager.DeleteStatus(StatusName.Chilliness);
        Managers.GetPlayer().AddShield(cnt * GetAbliltiValue(EffectType.Defence).RoundToInt());
    }

    public override object Clone()
    {
        AbsorptionChilliness rune = new AbsorptionChilliness();
        rune.Init();
        rune.UnEnhance();
        return rune;
    }
}
