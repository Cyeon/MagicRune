using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEAddDamage : StatusEvent
{
    private enum AddDamageType
    {
        Int,
        Stack
    }

    [SerializeField] private AddDamageType _addDamageType = AddDamageType.Int;

    [SerializeField, ConditionalField(nameof(_addDamageType), false, AddDamageType.Int)] private int _damage = 0;
    [SerializeField, ConditionalField(nameof(_addDamageType), false, AddDamageType.Stack)] private int _stackMultipleValue = 1;

    public override void Invoke()
    {
        if(_addDamageType == AddDamageType.Stack)
        {
            _damage = _status.TypeValue * _stackMultipleValue;
        }
        _unit.currentDmg += _damage;
    }
}
