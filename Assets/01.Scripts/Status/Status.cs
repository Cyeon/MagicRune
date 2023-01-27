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
    public StatusType type;
    public int typeValue;
    public UnityEvent statusFunc;
    public Sprite icon;

    public UnityEvent addFunc;

    public Status(Status status)
    {
        this.statusName = status.statusName;
        this.invokeTime = status.invokeTime;
        this.typeValue = status.typeValue;
        this.statusFunc = status.statusFunc;
        this.icon = status.icon;
        this.type = status.type;
    }
}