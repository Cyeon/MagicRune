using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAttack : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Resource.Load<BaseRuneSO>("SO/Rune/Ground/" + typeof(ShieldAttack).Name);
        base.Init();
    }
    public override void AbilityAction()
    {
        Managers.GetPlayer().AddShield(GetAbliltiValue(EffectType.Defence));
        BattleManager.Instance.Enemy.StatusManager.AddStatus(StatusName.Impact, GetAbliltiValue(EffectType.Status, StatusName.Impact).RoundToInt());
    }

    public override object Clone()
    {
        ShieldAttack shieldAttack = new ShieldAttack();
        shieldAttack.Init();
        shieldAttack.UnEnhance();
        return shieldAttack;
    }
}
