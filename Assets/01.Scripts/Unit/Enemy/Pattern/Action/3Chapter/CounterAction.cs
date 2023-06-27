using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterAction : PatternAction
{
    private int _damage = 0;

    public override void StartAction()
    {
        _damage = Enemy.PatternManager.BeforePattern.GetComponent<CombitReadinessAction>().absorbDamage;
        Enemy.PatternManager.CurrentPattern.desc = _damage.ToString();
        Enemy.PatternManager.UpdatePatternUI();

        base.StartAction();
    }

    public override void TurnAction()
    {
        Enemy.Attack(_damage);
        base.TurnAction();
    }
}
