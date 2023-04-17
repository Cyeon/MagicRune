using MyBox;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

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

public class Status : MonoBehaviour
{
    public string debugName = "";
    public StatusName statusName = StatusName.Null;
    [TextArea(1, 5)]
    public string information = "";
    public Color textColor = Color.white;

    [Header("Type")]
    public StatusType type = StatusType.Stack;
    private int _typeValue = 0;
    public int TypeValue => _typeValue;
    [ConditionalField(nameof(type), false, StatusType.Stack)]
    public bool isTurnRemove = false;

    [Header("Function")]
    public List<StatusEvent> OnAddStatus = new List<StatusEvent>();
    public List<StatusEvent> OnTurnStart = new List<StatusEvent>();
    public List<StatusEvent> OnAttack = new List<StatusEvent>();
    public List<StatusEvent> OnGetDamage = new List<StatusEvent>();
    public List<StatusEvent> OnTurnEnd = new List<StatusEvent>();

    [Header("Resource")]
    [ShowAssetPreview(32, 32)] public Sprite icon;
    public Color color = Color.white;

    [HideInInspector] public Unit unit;

    public void AddValue(int count)
    {
        _typeValue += count;
        Define.DialScene?.ReloadStatusPanel(unit, this);
    }

    public void RemoveValue(int count)
    {
        _typeValue = Mathf.Clamp(_typeValue - count, 0, _typeValue);
        if(_typeValue <= 0) 
        { 
            unit.StatusManager.DeleteStatus(this);
            return;
        }

        Define.DialScene?.ReloadStatusPanel(unit, this);
    }
}