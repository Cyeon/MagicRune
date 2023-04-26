using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestAction : PatternAction
{
    public override void TurnAction()
    {
        BattleManager.Instance.Enemy.PatternManager.CurrentPattern.NextAction();
    }
}
