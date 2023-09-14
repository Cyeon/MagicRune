using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEReflectionDamage : StatusEvent
{
    [SerializeField] private float _percent;

    public override void Invoke()
    {
        base.Invoke();

        if (_unit == null) return;
        _unit.attackDamage += Mathf.FloorToInt(_unit.attackDamage * (_percent * 0.01f));
        _unit.Attack();
    }
}
