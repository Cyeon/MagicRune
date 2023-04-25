using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefByDamageAttackAction : AttackAction
{
    public override void TurnAction()
    {
        damage = Mathf.FloorToInt(BattleManager.Instance.Enemy.Shield);
        count = 1;

        base.TurnAction();
    }
}
