using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackDebufferThreeToOneAction : StatusAction
{
    public override void TakeAction()
    {
        value = Mathf.RoundToInt(BattleManager.Instance.Enemy.StatusManager.GetStatus(StatusName.Coldness).TypeValue * 0.3f);
        BattleManager.Instance.Enemy.StatusManager.DeleteStatus(StatusName.Coldness);
        base.TakeAction();
    }
}
