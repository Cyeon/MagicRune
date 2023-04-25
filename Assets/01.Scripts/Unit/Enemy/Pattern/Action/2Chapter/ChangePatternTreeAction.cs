using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePatternTreeAction : PatternAction
{
    public string treeName;

    public override void TurnAction()
    {
        BattleManager.Instance.Enemy.PatternManager.ChangeTree(treeName);
        base.TurnAction();
    }
}
