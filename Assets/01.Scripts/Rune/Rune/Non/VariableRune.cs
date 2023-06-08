using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableRune : BaseRune
{
    [HideInInspector]
    public BaseRune nextRune;

    public override void Init()
    {
        base.Init();
        _baseRuneSO = Managers.Resource.Load<BaseRuneSO>("SO/Rune/Non/" + typeof(VariableRune).Name);
    }

    public override void AbilityAction()
    {
        if(nextRune.GetAbliltiValue(EffectType.Attack) != 0)
        {
            Managers.GetPlayer().Attack(GetAbliltiValue(EffectType.Attack), false);
        }
        else if(nextRune.GetAbliltiValue(EffectType.Defence) != 0)
        {
            Managers.GetPlayer().AddShield(GetAbliltiValue(EffectType.Defence));
        }
    }

    public override object Clone()
    {
        VariableRune variableRune = new VariableRune();
        variableRune.Init();
        variableRune.UnEnhance();
        return variableRune;
    }
}
