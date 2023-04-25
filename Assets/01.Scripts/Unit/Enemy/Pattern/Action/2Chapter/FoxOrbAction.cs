using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxOrbAction : PatternAction
{
    private bool _isGetDamage = false;

    public override void StartAction()
    {
        BattleManager.Instance.Enemy.OnGetDamage += () => _isGetDamage = true;
        base.StartAction();
    }

    public override void TurnAction()
    {
        if(!_isGetDamage)
        {
            BattleManager.Instance.Enemy.StatusManager.AddStatus(StatusName.FoxOrb, 1);
        }

        base.TurnAction();
    }

    public override void EndAction()
    {
        BattleManager.Instance.Enemy.OnGetDamage -= () => _isGetDamage = true;
        _isGetDamage = false;
        base.EndAction();
    }
}
