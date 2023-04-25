using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RemoveType
{ 
    Int,
    Percent
}

public class SERemoveDamage : StatusEvent
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
            _unit.currentDmg -= (int)(_unit.currentDmg * (0.01f * _percent));
        }
        else
        {
            _unit.currentDmg -= _value;
        }
    }
}
