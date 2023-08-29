using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableRune : BaseRune
{
    [HideInInspector]
    public BaseRune nextRune;

    public override void Init()
    {
        _baseRuneSO = Managers.Addressable.Load<BaseRuneSO>("SO/Rune/Non/" + typeof(VariableRune).Name);
        base.Init();
    }

    public override void AbilityAction()
    {
        if (nextRune is VariableRune || nextRune == null) return;

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
