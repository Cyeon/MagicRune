using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceShield : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Resource.Load<BaseRuneSO>("SO/Rune/Ice/" + typeof(IceShield).Name);
    }
    public override void AbilityAction()
    {
        Managers.GetPlayer().AddShield(GetAbliltiValaue(EffectType.Defence));
        BattleManager.Instance.Enemy.StatusManager.AddStatus(StatusName.IceShield, GetAbliltiValaue(EffectType.Status).RoundToInt());
    }
}
