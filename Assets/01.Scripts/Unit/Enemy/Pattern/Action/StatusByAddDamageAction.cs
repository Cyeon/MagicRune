using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusByAddDamageAction : PatternAction
{
    [SerializeField] private StatusName _status;
    [SerializeField] private int _multiple = 1;
    [SerializeField] private bool _isDelete = true;

    public override void DamageApplyAction()
    {
        int value = Enemy.StatusManager.GetStatusValue(_status);

        if(value > 0)
            Enemy.attackDamage += value * _multiple;

        if(_isDelete)
            Enemy.StatusManager.DeleteStatus(_status);

        base.DamageApplyAction();
    }
}
