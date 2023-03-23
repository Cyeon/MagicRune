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
    public List<Pair> EffectList => _effectList;

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

        _effectList = SortingEffect();
    }

    private List<Pair> SortingEffect()
    {
        List<Pair> sortingList = new List<Pair>();

        if (_effectList.Count == 0) return _effectList;

        for(int i = 0; i < (int)EffectType.Etc; i++)
        {
            for(int j = 0; j < _effectList.Count; j++)
            {
                if (_effectList[j].EffectType == (EffectType)i)
                {
                    sortingList.Add(_effectList[j]);
                }
            }
        }

        return sortingList;
    }

    private void Clear()
    {
        _effectList.Clear();
    }

    public void AddEffect(Pair effect)
    {
        _effectList.Add(effect);

        _effectList = SortingEffect();
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