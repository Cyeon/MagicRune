using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEAddShield : StatusEvent
{
    private enum AddShieldType
    {
        Int,
        Stack
    }

    [SerializeField] private AddShieldType _addShieldType = AddShieldType.Int;

    [SerializeField, ConditionalField(nameof(_addShieldType), false, AddShieldType.Int)] private int _shield = 0;
    [SerializeField, ConditionalField(nameof(_addShieldType), false, AddShieldType.Stack)] private int _stackMultipleValue = 1;

    public override void Invoke()
    {
        base.Invoke();

        if (_addShieldType == AddShieldType.Stack)
        {
            _shield = _status.TypeValue * _stackMultipleValue;
        }

        _unit.AddShield(_shield);
    }
}
