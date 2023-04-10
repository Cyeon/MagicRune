using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MyBox;

public class BaseRune // 일단 모노비헤이비어 뺴고 생각하자.
{
    protected BaseCardSO _baseCardSO;

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
            float value = _baseCardSO.AbilityList.Where(x => x.EffectType == EffectType.Attack).Select(x => x.Value).First();

            Managers.StatModifier.GetStatModifierValue(ref value);

            Managers.GetPlayer().Attack(value);
            Managers.Gold.AddGold(-1 * 5);
        }
    }
}
