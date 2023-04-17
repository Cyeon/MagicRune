using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemStackEvent : StatusEvent
{
    private enum RemStackType
    {
        Int,
        Dmg
    }

    [SerializeField] private RemStackType _remStackType = RemStackType.Int;

    [SerializeField, ConditionalField(nameof(_remStackType), false, RemStackType.Int)] 
    private int _value = 1;

    public override void Invoke()
    {
        if(_remStackType == RemStackType.Dmg)
        {
            _value = Mathf.FloorToInt(_unit.currentDmg);
        }
        _status.RemoveValue(_value);
    }
}
