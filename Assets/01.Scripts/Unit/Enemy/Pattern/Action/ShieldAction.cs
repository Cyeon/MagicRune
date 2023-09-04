using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAction : PatternAction
{
    public int amount;

    public override string Description => "<color=#369AC2>" + amount + "</color> <color=#F9B41F>방어</color>를 획득";

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
