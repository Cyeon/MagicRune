using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEOverheat : StatusEvent
{
    [SerializeField] private int _conditionCount = 10;

    public override void Invoke()
    {
        if (_status.TypeValue >= _conditionCount)
        {
            _unit.StatusManager.AddStatus(StatusName.Fire, _status.TypeValue / 2);
            _unit.StatusManager.DeleteStatus(_status);
        }
    }
}
