using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAttack : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Resource.Load<BaseRuneSO>("SO/Rune/Ground/" + typeof(ShieldAttack).Name);
    }
    public override void AbilityAction()
    {
        Managers.GetPlayer().AddShield(GetAbliltiValaue(EffectType.Defence));
        BattleManager.Instance.Enemy.StatusManager.AddStatus(StatusName.Impact, GetAbliltiValaue(EffectType.Status).RoundToInt());
    }

    public override object Clone()
    {
        ShieldAttack shieldAttack = new ShieldAttack();
        shieldAttack.Init();
        return shieldAttack;
    }
}
