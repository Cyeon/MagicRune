using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackDebufferThreeToOneAction : StatusAction
{
    [SerializeField] private StatusName _statusName = StatusName.Coldness;

    public override void TakeAction()
    {
        if(BattleManager.Instance.Enemy.StatusManager.GetStatus(_statusName) != null)
        {
            value = Mathf.RoundToInt(BattleManager.Instance.Enemy.StatusManager.GetStatus(_statusName).TypeValue * 0.3f);
            BattleManager.Instance.Enemy.StatusManager.DeleteStatus(_statusName);
        }

        base.TakeAction();
    }
}
