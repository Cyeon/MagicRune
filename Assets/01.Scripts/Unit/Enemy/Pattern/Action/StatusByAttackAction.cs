using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusByAttackAction : PatternAction
{
    [SerializeField] private StatusName _status;

    public override void DamageApplyAction()
    {
        Enemy.attackDamage += Enemy.StatusManager.GetStatusValue(_status);
        Enemy.StatusManager.DeleteStatus(_status);

        base.DamageApplyAction();
    }
}
