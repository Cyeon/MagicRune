using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackDebufferThreeToOneAction : StatusAction
{
    public override void TakeAction()
    {
        value = Mathf.RoundToInt(StatusManager.Instance.GetUnitStatusValue(BattleManager.Instance.enemy, StatusName.Coldness) * 0.3f);
        StatusManager.Instance.DeleteStatus(BattleManager.Instance.enemy, StatusName.Coldness);
        base.TakeAction();
    }
}
