using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Pattern
{
    public string patternName;
    public Sprite icon;
    public UnityEvent patternStartFunc;
    public UnityEvent patternTurnFunc;
    public UnityEvent patternEndFunc;

    public void Start()
    {
        UIManager.Instance.ReloadPatternIcon(icon);
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
