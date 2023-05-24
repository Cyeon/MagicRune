using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum StatModifierType
{
    // 이 순서대로 연산 처리함

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
                _statModifierDict[(EffectType)i].Add((StatModifierType)j, 0);
            }
        }
    }

    public void AddStat(EffectType effectType, StatModifierType type, float value)
    {
        _statModifierDict[effectType][type] += value;
    }

    public void SubtractStat(EffectType effectType, StatModifierType type, float value)
    {
        if (value == 0)
        {
            _statModifierDict[effectType][type] = 0;
        }
        else
        {
            _statModifierDict[effectType][type] -= value;
        }
    }

    public void RemoveStat(EffectType effectType, StatModifierType type)
    {
        _statModifierDict[effectType][type] = 0;
    }

    public void GetStatModifierValue(EffectType effectType, ref float? value)
    {
        if (_statModifierDict.ContainsKey(effectType) == true)
        {
            foreach (var stat in _statModifierDict[effectType])
            {
                Debug.Log(effectType + ", " + stat.Key + "," + stat.Value);
                if (stat.Value != 0)
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
    }

    public void Clear()
    {
        Debug.Log("초기화");
        _statModifierDict.Clear();
    }
}
