using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum StatusInvokeTime
{
    Start,
    Attack,
    End
}

[System.Serializable]
public class Status
{
    public string statusName;
    public  StatusInvokeTime invokeTime;
    public int durationTurn;
    public UnityEvent statusFunc;

    [HideInInspector] public int currentTurn;

    public Status(Status status)
    {
        this.statusName = status.statusName;
        this.invokeTime = status.invokeTime;
        this.durationTurn = status.durationTurn;
        this.currentTurn = status.durationTurn;
        this.statusFunc = status.statusFunc;
    }

    public void StatusInvoke()
    {
        statusFunc.Invoke();
        currentTurn -= 1;
        if(currentTurn <= 0)
        {
            StatusManager.Instance.RemStatus(GameManager.Instance.currentUnit, this);
        }
    }
}