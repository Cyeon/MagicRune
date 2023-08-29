using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroadeningDescChangeAction : AttackAction
{
    private Broadening _broadening;

    public override void StartAction()
    {
        base.StartAction();

        if(_broadening != null)
        {
            _broadening = Enemy.PatternManager.passive as Broadening;
        }

        Enemy.PatternManager.CurrentPattern.ChangePatternDescription(damage + _broadening.AddDmg.ToString());
    }
}
