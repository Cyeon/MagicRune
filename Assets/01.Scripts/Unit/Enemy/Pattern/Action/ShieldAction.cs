using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAction : PatternAction
{
    public int amount;

    public override void TakeAction()
    {
        BattleManager.Instance.Enemy.AddShield(amount);
        BattleManager.Instance.Enemy.PatternManager.CurrentPattern.NextAction();
    }
}
