using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusByHealAction : PatternAction
{
    [SerializeField] private StatusName _status;
    private bool _isRemoveStack;

    public override void TurnAction()
    {
        Enemy.AddHP(Enemy.StatusManager.GetStatusValue(_status) / 2);
        if (_isRemoveStack)
            Enemy.StatusManager.DeleteStatus(_status);

        base.TurnAction();
    }
}
