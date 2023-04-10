using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackDamageEvent : DamageEvent
{
    public override void Invoke()
    {
        _damage = _status.TypeValue;
        base.Invoke();
    }
}
