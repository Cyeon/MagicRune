using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddDmgEvent : StatusEvent
{
    [SerializeField] protected int _damage;

    public override void Invoke()
    {
        _unit.currentDmg += _damage;
    }
}
