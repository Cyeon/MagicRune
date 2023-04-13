using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackAddDmgEvent : AddDmgEvent
{
    [SerializeField] private int _addValue;

    public override void Invoke()
    {
        _damage = _addValue * _status.TypeValue;
        base.Invoke();
    }
}
