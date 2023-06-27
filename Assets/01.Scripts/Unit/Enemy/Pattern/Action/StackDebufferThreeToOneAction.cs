using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackDebufferThreeToOneAction : StatusAction
{
    [Header("Condition 조건")]
    [SerializeField] private StatusName _checkStatusName = StatusName.ChillinessZip;

    public override void TurnAction()
    {
        if (Enemy.StatusManager.GetStatus(_checkStatusName) != null)
        {
            value = Mathf.RoundToInt((float)Enemy.StatusManager.GetStatus(_checkStatusName).TypeValue * 0.3f);
            if (value > 0) value++;
        }
        Enemy.StatusManager.DeleteStatus(_checkStatusName);
        base.TurnAction();
    }
}
