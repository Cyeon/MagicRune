using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEDeleteStatus : StatusEvent
{
    public override void Invoke()
    {
        base.Invoke();

        _status.RemoveValue(_status.TypeValue);
    }
}
