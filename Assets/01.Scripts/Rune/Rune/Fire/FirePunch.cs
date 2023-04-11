using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePunch : BaseRune
{
    public override bool AbilityCondition()
    {
        return true;
    }

    public override void AbilityAction()
    {
        Managers.GetPlayer().Attack(GetAbliltiValaue(EffectType.Attack));
        StatusManager.Instance.AddStatus(BattleManager.Instance.enemy, StatusName.Fire, 4);
    }
}
