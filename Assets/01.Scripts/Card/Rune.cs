using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum OutlineType
{
    Default,
    Cyan,
    Red,
    COUNT
}

[Serializable]
public class Rune
{
    [SerializeField, DisplayInspector]
    private RuneSO _runeSO;

    private List<Pair> _effectList = new List<Pair>();

    private int _coolTime;
    public bool IsCoolTime => _coolTime > 0;

    public Rune(RuneSO rune)
    {
        _runeSO = rune;

        SettingEffect();
    }

    public RuneSO GetRune()
    {
        return _runeSO;
    }

    public void SetRune(RuneSO rune)
    {
        _runeSO = rune;

        SettingEffect();
    }

    private void SettingEffect()
    {
        Clear();
        _effectList = new List<Pair>(_runeSO.MainRune.EffectDescription);
    }

    private void Clear()
    {
        _effectList.Clear();
    }

    public void AddEffect(Pair effect)
    {
        _effectList.Add(effect);
    }

    public void SetCoolTime(int cooltime)
    {
        _coolTime = cooltime;

        // ÈÄÃ³¸®
    }

    public int GetCoolTime()
    {
        return _coolTime;
    }
}