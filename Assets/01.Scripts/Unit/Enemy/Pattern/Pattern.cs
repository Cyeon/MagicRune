using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Pattern
{
    public string patternName;
    public Sprite icon;
    public float value;
    public UnityEvent patternStartFunc;
    public UnityEvent patternTurnFunc;
    public UnityEvent patternEndFunc;

    public void Start()
    {
        UIManager.Instance.ReloadPattern(icon, value.ToString());
        PatternManager.Instance.funcList.value = value;
        patternStartFunc?.Invoke();
    }

    public void Turn()
    {
        patternTurnFunc.Invoke();
    }

    public void End()
    {
        patternEndFunc?.Invoke();
    }
}
