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
        StatusManager.Instance.AddStatus(isSelf ? BattleManager.Instance.enemy : BattleManager.Instance.player, status, value);
        BattleManager.Instance.TurnChange();
    }
}
