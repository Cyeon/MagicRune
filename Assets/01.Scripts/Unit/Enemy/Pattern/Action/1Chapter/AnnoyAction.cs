using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnoyAction : PatternAction
{
    [SerializeField]
    private AttackAction _attackAction;

    public override void StartAction()
    {
        base.StartAction();

        Enemy.PatternManager.CurrentPattern.desc = _attackAction.damage + (Managers.Enemy.CurrentEnemy.StatusManager.GetStatusValue(StatusName.Annoy) * 5).ToString();
        Enemy.PatternManager.UpdatePatternUI();
    }
}
