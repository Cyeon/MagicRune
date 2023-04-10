using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RemoveType
{ 
    Percent,
    Int
}

public class RemDmgEvent : StatusEvent
{
    public RemoveType removeType = RemoveType.Int;
    [SerializeField, ConditionalField(nameof(removeType), false, RemoveType.Percent)]
    private float _percent = 0;
    [SerializeField, ConditionalField(nameof(removeType), false, RemoveType.Int)]
    private int _value = 0;

    public override void Invoke()
    {
        if(removeType == RemoveType.Percent)
        {
            _unit.currentDmg -= _unit.currentDmg * (0.01f * _percent);
        }
        else
        {
            _unit.currentDmg -= _value;
        }
    }
}
