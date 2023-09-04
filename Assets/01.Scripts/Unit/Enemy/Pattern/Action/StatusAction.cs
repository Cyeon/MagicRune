using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusAction : PatternAction
{
    public StatusName status;
    public int value;
    public bool isSelf;

    public override string Description
    {
        get
        {
            if(isSelf)
            {
                return Define.BENEFIC_DESC;
            }

            return Define.INJURIOUS_DESC;
        }
    }

    public override void StartAction()
    {
        Status();
        base.StartAction();
    }

    public override void TurnAction()
    {
        Status();
        base.TurnAction();
    }

    private void Status()
    {
        Unit unit = isSelf ? BattleManager.Instance.Enemy : BattleManager.Instance.Player;

        if (value < 0)
        {
            unit.StatusManager.RemoveStatus(status, value * -1);
        }
        else if (value > 0)
        {
            unit.StatusManager.AddStatus(status, value);
        }
    }
}
