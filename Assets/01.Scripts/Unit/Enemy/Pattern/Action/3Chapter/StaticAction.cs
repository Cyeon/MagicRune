using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticAction : PatternAction
{
    private bool _isGetDamage = false;

    public override void StartAction()
    {
        _isGetDamage = false;
        BattleManager.Instance.Enemy.OnGetDamage += AddStrength;
        base.StartAction();
    }

    public override void EndAction()
    {
        BattleManager.Instance.Enemy.OnGetDamage -= AddStrength;

        base.EndAction();
    }

    private void AddStrength()
    {
        if (!_isGetDamage)
        {
            _isGetDamage = true;
            BattleManager.Instance.Enemy.StatusManager.AddStatus(StatusName.Strength, 10);
        }
    }
}
