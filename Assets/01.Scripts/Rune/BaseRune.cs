using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MyBox;

public class BaseRune // �ϴ� �������̺�� ���� ��������.
{
    protected BaseCardSO _baseCardSO;

    public virtual void Init()
    {
        _baseCardSO = Managers.Resource.Load<BaseCardSO>("SO/Rune/" + typeof(BaseCardSO).Name + "SO");
    }

    public virtual bool AbilityCondition()
    {
        bool isHaveGold = Managers.Gold.Gold >= 5;

        return isHaveGold;
    }

    public virtual void AbilityAction()
    {
        // ���� ��...

        // 5��� �̻��϶� 5��带 �Һ��ϰ� ������ �ϴ� ���
        if(AbilityCondition())
        {
            float value = _baseCardSO.AbilityList.Where(x => x.EffectType == EffectType.Attack).Select(x => x.Value).First();

            Managers.StatModifier.GetStatModifierValue(ref value);

            Managers.GetPlayer().Attack(value < 0 ? 0 : value);
            Managers.Gold.AddGold(-1 * 5);
        }
    }

    public float GetAbliltiValaue(EffectType type)
    {
        float value = _baseCardSO.AbilityList.Where(x => x.EffectType == type).Select(x => x.Value).First();

        Managers.StatModifier.GetStatModifierValue(ref value);

        return value;
    }
}
