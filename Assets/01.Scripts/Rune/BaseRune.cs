using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MyBox;
using System;

[Serializable]
public class BaseRune : MonoBehaviour
{
    [SerializeField]
    protected BaseRuneSO _baseCardSO;

    private int _coolTime;

    public bool IsCoolTIme => _coolTime > 0;

    public void SetCoolTime()
    {
        _coolTime = _baseCardSO.CoolTime;
    }

    public void AddCoolTime(int value)
    {
        _coolTime += value;
    }

    public virtual bool AbilityCondition()
    {
        bool isHaveGold = Managers.Gold.Gold >= 5;

        return isHaveGold;
    }

    public virtual void AbilityAction()
    {
        // 예시 용...

        // 5골드 이상일때 5골드를 소비하고 공격을 하는 경우
        float value = GetAbliltiValaue(EffectType.Attack);

        Managers.GetPlayer().Attack(value == int.MinValue ? 0 : value);
        Managers.Gold.AddGold(-1 * 5);
    }

    public float GetAbliltiValaue(EffectType type)
    {
        float? value = _baseCardSO.AbilityList.Where(x => x.EffectType == type).Select(x => x.Value).FirstOrDefault();

        if (value.HasValue)
        {
            Managers.StatModifier.GetStatModifierValue(type, ref value);

            return value.Value;
        }

        return int.MinValue;
    }
}
