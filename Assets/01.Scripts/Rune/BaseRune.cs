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

    private void Start()
    {
        Init();
    }

    public virtual void Init()
    {
        _baseCardSO = Managers.Resource.Load<BaseRuneSO>("SO/Rune/" + typeof(BaseRuneSO).Name);
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
        if(AbilityCondition())
        {
            float value = GetAbliltiValaue(EffectType.Attack);

            Managers.GetPlayer().Attack(value == int.MinValue ? 0 : value);
            Managers.Gold.AddGold(-1 * 5);
        }
    }

    public float GetAbliltiValaue(EffectType type)
    {
        float? value = _baseCardSO.AbilityList.Where(x => x.EffectType == type).Select(x => x.Value).FirstOrDefault();

        if (value.HasValue)
        {
            Managers.StatModifier.GetStatModifierValue(ref value);

            return value.Value;
        }

        return int.MinValue;
    }
}
