using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEFire : StatusEvent
{
    [SerializeField] private GameTurn _turnType;

    public override void Invoke()
    {
        if (BattleManager.Instance.GameTurn == _turnType)
        {
            _unit.TakeDamage(_status.TypeValue, true, _status);
            _unit.StatusManager.RemoveStatus(_status.statusName, _status.TypeValue / 2);
        }
    }
}
