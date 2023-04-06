using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

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
    Ice,
    Wound,
    Recharging,
    Coldness, // 냉기
    Chilliness, // 한기
    BladeOfKnife,
    COUNT
}

[System.Serializable]
public class Status
{
    public string debugName = "";
    [Header("Information")]
    public StatusName statusName = StatusName.Null;
    public string information = "";
    public Color textColor = Color.white;

    [Header("Type")]
    public  StatusInvokeTime invokeTime = StatusInvokeTime.Start;
    public StatusType type = StatusType.Stack;
    public int typeValue = 0;
    public bool isTurnRemove = false;

    [Header("Function")]
    public UnityEvent statusFunc;
    public UnityEvent addFunc;

    [Header("Resource")]
    [ShowAssetPreview(32, 32), Tooltip("�̹���")]
    public Sprite icon;
    public Color color = Color.white;

    [HideInInspector] public Unit unit;

    public Status(Status status)
    {
        this.statusName = status.statusName;
        this.debugName = status.debugName;
        this.information = status.information;
        this.textColor = status.textColor;

        this.invokeTime = status.invokeTime;
        this.typeValue = status.typeValue;
        this.isTurnRemove = status.isTurnRemove;

        this.unit = status.unit;

        this.statusFunc = status.statusFunc;
        this.type = status.type;
        this.addFunc = status.addFunc;

        this.icon = status.icon;
        this.color = status.color;
    }

    
}