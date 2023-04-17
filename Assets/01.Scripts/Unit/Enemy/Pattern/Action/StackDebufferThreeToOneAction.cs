using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackDebufferThreeToOneAction : StatusAction
{
    [SerializeField] private StatusName _statusName = StatusName.Coldness;

    public override void TakeAction()
    {
        if(BattleManager.Instance.Enemy.StatusManager.GetStatus(StatusName.Coldness) != null)
        {
            value = Mathf.RoundToInt(BattleManager.Instance.Enemy.StatusManager.GetStatus(StatusName.Coldness).TypeValue * 0.3f);
            BattleManager.Instance.Enemy.StatusManager.DeleteStatus(StatusName.Coldness);
        }

        base.TakeAction();
    }
}
