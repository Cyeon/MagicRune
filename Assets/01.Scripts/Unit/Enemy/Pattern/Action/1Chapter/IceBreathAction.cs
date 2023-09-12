using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBreathAction : StatusAction
{
    [SerializeField] private StatusName _conditionStatus;
    [SerializeField] private int _applyCount = 5;

    public override void TurnAction()
    {
        if(Managers.GetPlayer().StatusManager.IsHaveStatus(_conditionStatus))
        {
            value = _applyCount;
        }

        base.TurnAction();
    }
}
