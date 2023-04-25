using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEAttack : StatusEvent
{
    [SerializeField] private float _damage;

    public override void Invoke()
    {
        _unit.Attack(_damage);
    }
}
