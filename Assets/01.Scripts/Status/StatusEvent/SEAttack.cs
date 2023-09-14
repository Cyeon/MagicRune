using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEAttack : StatusEvent
{
    [SerializeField] private int _damage;

    public override void Invoke()
    {
        base.Invoke();
        _unit.attackDamage = _damage;
        _unit.Attack();
    }
}
