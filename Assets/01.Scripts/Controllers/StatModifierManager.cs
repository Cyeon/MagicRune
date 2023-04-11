using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatModifierType
{
    Add,
    Subtract,
    Multiply,
    Divide,
    END,
}

public class StatModifierManager
{
    private Dictionary<EffectType, Dictionary<StatModifierType, float>> _statModifierDict = new Dictionary<EffectType, Dictionary<StatModifierType, float>>();
    public Dictionary<EffectType, Dictionary<StatModifierType, float>> StatModifierDict => _statModifierDict;

    public void Init()
    {
        Clear();

        for (int i = 0; i < (int)EffectType.Etc; i++)
        {
            _statModifierDict.Add((EffectType)i, new Dictionary<StatModifierType, float>());
            for (int j = 0; j < (int)StatModifierType.END; j++)
            {
                _statModifierDict[(EffectType)i].Add((StatModifierType)j, int.MinValue);
            }
        }


    }

    public void AddStat(EffectType effectType, StatModifierType type, float value)
    {
        _statModifierDict[effectType][type] += value;
    }

    public void SubtractStat(EffectType effectType, StatModifierType type, float value)
    {
        if (value == int.MinValue)
        {
            _statModifierDict[effectType][type] = int.MinValue;
        }
        else
        {
            _statModifierDict[effectType][type] -= value;
        }
    }

    public void RemoveStat(EffectType effectType, StatModifierType type)
    {
        _statModifierDict[effectType][type] = int.MinValue;
    }

    public void GetStatModifierValue(EffectType effectType, ref float? value)
    {
        foreach (var stat in _statModifierDict[effectType])
        {
            if (stat.Value != int.MinValue)
            {
                switch (stat.Key)
                {
                    case StatModifierType.Add:
                        value += stat.Value;
                        break;
                    case StatModifierType.Subtract:
                        value -= stat.Value;
                        break;
                    case StatModifierType.Multiply:
                        value *= stat.Value;
                        break;
                    case StatModifierType.Divide:
                        value -= stat.Value;
                        break;
                    case StatModifierType.END:
                        break;
                }
                value = Mathf.Floor(value.Value);
            }
        }
    }

    public void Clear()
    {
        _statModifierDict.Clear();
    }
}
