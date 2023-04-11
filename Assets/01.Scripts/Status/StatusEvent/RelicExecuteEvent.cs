using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicExecuteEvent : StatusEvent
{
    [SerializeField] private string _relicName;

    public override void Invoke()
    {
        if(_unit.IsPlayer)
            Managers.Relic.ContinuousExecute(_relicName);
    }
}
