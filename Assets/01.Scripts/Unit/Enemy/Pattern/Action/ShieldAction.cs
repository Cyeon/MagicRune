using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAction : PatternAction
{
    public int amount;

    public override void StartAction()
    {
        BattleManager.Instance.Enemy.AddShield(amount);
        BattleManager.Instance.Enemy.PatternManager.CurrentPattern.NextAction();

        base.StartAction();
    }

    public override void TurnAction()
    {
        BattleManager.Instance.Enemy.AddShield(amount);
        BattleManager.Instance.Enemy.PatternManager.CurrentPattern.NextAction();

        base.TurnAction();
    }
}
