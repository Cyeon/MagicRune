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
    private Dictionary<StatModifierType, float> _statModifierDict = new Dictionary<StatModifierType, float>();
    public Dictionary<StatModifierType, float> StatModifierDict => _statModifierDict;

    public void Init()
    {
        Clear();

        for (int i = 0; i < (int)StatModifierType.END; i++)
        {
            _statModifierDict.Add((StatModifierType)i, int.MinValue);
        }
    }

    public void AddStat(StatModifierType type, float value)
    {
        _statModifierDict[type] += value;
    }

    public void SubtractStat(StatModifierType type, float value)
    {
        _statModifierDict[type] -= value;
    }

    public void GetStatModifierValue(ref float value)
    {
        foreach (var stat in _statModifierDict)
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
            }
        }
    }

    public void Clear()
    {
        _statModifierDict.Clear();
    }
}
