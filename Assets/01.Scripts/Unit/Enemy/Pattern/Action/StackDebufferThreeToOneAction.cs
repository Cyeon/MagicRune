using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackDebufferThreeToOneAction : StatusAction
{
    [Header("Condition 조건")]
    [SerializeField] private StatusName _checkStatusName = StatusName.Coldness;

    public override void TurnAction()
    {
        if(BattleManager.Instance.Enemy.StatusManager.GetStatus(_checkStatusName) != null)
        {
            value = Mathf.RoundToInt(BattleManager.Instance.Enemy.StatusManager.GetStatus(_checkStatusName).TypeValue * 0.3f);
            BattleManager.Instance.Enemy.StatusManager.DeleteStatus(_checkStatusName);
        }

        base.TurnAction();
    }
}
