using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroadeningDescChangeAction : AttackAction
{
    private Broadening _broadening;

    public override void StartAction()
    {

        if(_broadening != null)
        {
            _broadening = Enemy.PatternManager.passive as Broadening;
        }

        Enemy.PatternManager.CurrentPattern.ChangePatternValue(damage + _broadening.AddDmg.ToString());
        base.StartAction();
    }
}
