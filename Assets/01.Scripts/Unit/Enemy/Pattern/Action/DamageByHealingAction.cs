using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageByHealingAction : HealingAction
{
    public override void TurnAction()
    {
        _value = Mathf.FloorToInt(BattleManager.Instance.Player.currentDmg);
        base.TurnAction();
    }
}
