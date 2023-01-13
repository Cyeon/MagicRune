using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum StatusInvokeTime
{
    Start,
    Attack,
    GetDamage,
    End
}

[System.Serializable]
public class Status
{
    public string statusName;
    public  StatusInvokeTime invokeTime;
    public int durationTurn;
    public UnityEvent statusFunc;

    public Status(Status status)
    {
        this.statusName = status.statusName;
        this.invokeTime = status.invokeTime;
        this.durationTurn = status.durationTurn;
        this.statusFunc = status.statusFunc;
    }

    public void RemDuration(Unit unit)
    {
        durationTurn -= 1;
        if(durationTurn <= 0)
        {
            StatusManager.Instance.RemStatus(unit, this);
        }
    }
}