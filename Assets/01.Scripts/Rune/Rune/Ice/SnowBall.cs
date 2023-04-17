using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBall : BaseRune
{
    public override void AbilityAction()
    {
        Managers.GetPlayer().Attack(GetAbliltiValaue(EffectType.Attack));
        BattleManager.Instance.Enemy.StatusManager.AddStatus(StatusName.Chilliness, 2);
    }
}
