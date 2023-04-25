using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEDie : StatusEvent
{
    public override void Invoke()
    {
        _unit.Die();
    }
}
