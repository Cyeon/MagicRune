using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MyBox;

public class BaseRune // �ϴ� �������̺�� ���� ��������.
{
    protected BaseCardSO _baseCardSO;

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

            Managers.GetPlayer().Attack(value);
            Managers.Gold.AddGold(-1 * 5);
        }
    }
}
