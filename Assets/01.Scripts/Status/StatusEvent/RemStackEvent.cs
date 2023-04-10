using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemStackEvent : StatusEvent
{
    [SerializeField] private int _value = 1;

    public override void Invoke()
    {
        _status.RemoveValue(_value);
    }
}
