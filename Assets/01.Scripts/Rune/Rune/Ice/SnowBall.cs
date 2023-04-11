using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBall : BaseRune
{
    public override bool AbilityCondition()
    {
        return true;
    }

    public override void AbilityAction()
    {
        Managers.GetPlayer().Attack(GetAbliltiValaue(EffectType.Attack));
        StatusManager.Instance.AddStatus(BattleManager.Instance.enemy, StatusName.Ice, 2);
    }
}
