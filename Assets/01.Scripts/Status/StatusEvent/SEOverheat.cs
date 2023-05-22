using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEOverheat : StatusEvent
{
    [SerializeField] private int _lessThanCount = 5;
    [SerializeField] private int _overstackConditionCount = 10;

    public override void Invoke()
    {
        if (_status.TypeValue >= _overstackConditionCount)
        {
            _unit.StatusManager.AddStatus(StatusName.Fire, _status.TypeValue / 2);
            _unit.StatusManager.DeleteStatus(_status);
            _unit.StatusManager.DeleteStatus(StatusName.Heating);
        }

        if(_status.TypeValue < _lessThanCount)
        {
            _unit.StatusManager.DeleteStatus(StatusName.Heating);
        }
    }
}
