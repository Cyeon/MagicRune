using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefByDamageAttackAction : AttackAction
{
    public override void TakeAction()
    {
        damage = Mathf.FloorToInt(BattleManager.Instance.enemy.Shield);
        count = 1;

        base.TakeAction();
    }
}
