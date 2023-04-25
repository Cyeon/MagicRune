using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieAction : PatternAction
{
    public override void TurnAction()
    {
        BattleManager.Instance.Enemy.Die();
    }
}
