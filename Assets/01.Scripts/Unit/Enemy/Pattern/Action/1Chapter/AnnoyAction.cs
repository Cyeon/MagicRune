using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnoyAction : PatternAction
{
    [SerializeField]
    private AttackAction _attackAction;

    public override void StartAction()
    {
        if(Enemy.StatusManager.IsHaveStatus(StatusName.Annoy))
        {
            _attackAction.damage += 5;
        }

        Enemy.PatternManager.CurrentPattern.patternValue = _attackAction.damage.ToString();
        Enemy.PatternManager.UpdatePatternUI();
        base.StartAction();
    }
}
