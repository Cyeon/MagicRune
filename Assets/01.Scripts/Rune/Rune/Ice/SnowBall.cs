using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBall : BaseRune
{
    public override void AbilityAction()
    {
        Managers.GetPlayer().Attack(GetAbliltiValaue(EffectType.Attack));
        BattleManager.Instance.enemy.StatusManager.AddStatus(StatusName.Ice, 2);
    }
}
