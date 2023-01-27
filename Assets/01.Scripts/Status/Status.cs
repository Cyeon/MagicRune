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

public enum StatusType
{
    Stack,
    Turn
}

[System.Serializable]
public class Status
{
    public string statusName;
    public  StatusInvokeTime invokeTime;
    public Sprite icon;
    public StatusType type;
    public int durationTurn;
    public UnityEvent statusFunc;

    public Status(Status status)
    {
        this.statusName = status.statusName;
        this.invokeTime = status.invokeTime;
        this.durationTurn = status.durationTurn;
        this.statusFunc = status.statusFunc;
        this.icon = status.icon;
        this.type = status.type;
    }
}