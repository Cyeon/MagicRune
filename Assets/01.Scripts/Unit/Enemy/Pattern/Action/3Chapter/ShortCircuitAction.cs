using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortCircuitAction : PatternAction
{
    private bool _isGetDamage = false;

    public override void StartAction()
    {
        _isGetDamage = false;
        BattleManager.Instance.Enemy.OnGetDamage += GetDamage;
        base.StartAction();
    }

    public override void EndAction()
    {
        BattleManager.Instance.Enemy.OnGetDamage -= GetDamage;

        base.EndAction();
    }

    private void GetDamage()
    {
        if(!_isGetDamage)
        {
            _isGetDamage = true;
            BattleManager.Instance.Enemy.StatusManager.AddStatus(StatusName.Recharging, 10);
        }
    }
}
