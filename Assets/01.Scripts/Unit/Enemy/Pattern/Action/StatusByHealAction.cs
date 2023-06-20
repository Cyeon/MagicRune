using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusByHealAction : PatternAction
{
    [SerializeField] private StatusName _status;
    [SerializeField] private bool _isRemoveStack = true;

    public override void TurnAction()
    {
        Enemy.AddHP(Enemy.StatusManager.GetStatusValue(_status) / 2);
        if (_isRemoveStack)
            Enemy.StatusManager.DeleteStatus(_status);

        base.TurnAction();
    }
}
