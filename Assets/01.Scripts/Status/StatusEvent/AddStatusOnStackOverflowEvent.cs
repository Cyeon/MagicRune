using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddStatusOnStackOverflowEvent : StatusEvent
{
    [Header("Condition ����")]
    [SerializeField]
    private StatusName _conditionStatus = StatusName.Null;
    [SerializeField]
    public int _conditionCount;

    [Header("Result ���")]
    [SerializeField]
    private StatusName _addStatus = StatusName.Null;

    public override void Invoke()
    {
        if(_unit.StatusManager.GetStatus(_conditionStatus)?.TypeValue >= _conditionCount)
        {
            _unit.StatusManager.DeleteStatus(_conditionStatus);
            _unit.StatusManager.AddStatus(_addStatus, 1);
        }
    }
}
