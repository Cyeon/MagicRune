using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestAction : PatternAction
{
    public override void TakeAction()
    {
        BattleManager.Instance.enemy.PatternManager.CurrentPattern.NextAction();
    }
}
