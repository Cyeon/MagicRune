using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheLastShot : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Resource.Load<BaseRuneSO>("SO/Rune/Fire/" + typeof(TheLastShot).Name);
    }

    public override void AbilityAction()
    {
        BattleManager.Instance.Player.Attack(GetAbliltiValue(EffectType.Attack), false);
        BattleManager.Instance.Player.StatusManager.AddStatus(StatusName.OverHeat, (int)GetAbliltiValue(EffectType.Status));
    }

    public override object Clone()
    {
        TheLastShot fire = new TheLastShot();
        fire.Init();
        return fire;
    }
}
