using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefByDamageAttackAction : PatternAction
{
    public override void DamageApplyAction()
    {
        Enemy.attackDamage += Mathf.FloatToHalf(Enemy.Shield);

        base.DamageApplyAction();
    }
}
