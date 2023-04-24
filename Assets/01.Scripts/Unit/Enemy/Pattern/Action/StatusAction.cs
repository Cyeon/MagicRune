using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusAction : PatternAction
{
    public StatusName status;
    public int value;
    public bool isSelf;

    public override void TakeAction()
    {
        Unit unit = isSelf ? BattleManager.Instance.Enemy : BattleManager.Instance.Player;

        if(value < 0)
        {
            unit.StatusManager.RemoveStatus(status, value * -1);
        }
        else if (value > 0)
        {
            unit.StatusManager.AddStatus(status, value);
        }

        BattleManager.Instance.Enemy.PatternManager.CurrentPattern.NextAction();
    }
}
