using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEReflectionDamage : StatusEvent
{
    [SerializeField] private float _percent;

    public override void Invoke()
    {
        base.Invoke();

        float dmg = _unit.currentDmg * (_percent * 0.01f);
        _unit.Attack(dmg);
    }
}
