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
        if(value < 0)
            StatusManager.Instance.RemStatus(isSelf ? BattleManager.Instance.enemy : BattleManager.Instance.player, status, value);
        else
            StatusManager.Instance.AddStatus(isSelf ? BattleManager.Instance.enemy : BattleManager.Instance.player, status, value);

        BattleManager.Instance.enemy.patternM.currentPattern.NextAction();
    }
}
