using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEvent : StatusEvent
{
    [SerializeField] protected int _damage;
    public bool isTrueDamage = false;

    public override void Invoke()
    {
        _unit.TakeDamage(_damage, isTrueDamage, _status);
    }
}
