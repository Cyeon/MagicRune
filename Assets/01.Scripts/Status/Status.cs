using NaughtyAttributes;
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

public enum StatusName
{
    Null,
    Weak,
    Fire,
    Ice
}

[System.Serializable]
public class Status
{
    public StatusName statusName = StatusName.Null;
    public  StatusInvokeTime invokeTime = StatusInvokeTime.Start;
    public StatusType type = StatusType.Stack;
    public int typeValue = 0;

    [HideInInspector] public Unit unit;

    [Header("Function")]
    public UnityEvent statusFunc;
    public UnityEvent addFunc;

    [Header("Resource")]
    [ShowAssetPreview(32, 32), Tooltip("¿ÃπÃ¡ˆ")]
    public Sprite icon;
    public Color color;

    public Status(Status status)
    {
        this.statusName = status.statusName;
        this.invokeTime = status.invokeTime;
        this.typeValue = status.typeValue;

        this.unit = status.unit;

        this.statusFunc = status.statusFunc;
        this.type = status.type;
        this.addFunc = status.addFunc;

        this.icon = status.icon;
        this.color = status.color;
    }
}