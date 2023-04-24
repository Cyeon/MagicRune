using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEAddStatus : StatusEvent
{
    [SerializeField] private StatusName _statusName = StatusName.Null;
    [SerializeField] private int _count = 0;
    [SerializeField] private bool _isSelf = false;

    public override void Invoke()
    {
        Unit unit = _unit;
        if(!_isSelf)
        {
            if (_unit == Managers.GetPlayer()) unit = BattleManager.Instance.Enemy;
            else unit = Managers.GetPlayer();
        }

        unit.StatusManager.AddStatus(_statusName, _count);
    }
}
