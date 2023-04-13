using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundShield : BaseRune
{
    public override void AbilityAction()
    {
        Managers.GetPlayer().AddShield(GetAbliltiValaue(EffectType.Defence));
    }

    public override void Execute()
    {
        Debug.Log("Buy GroundShield Rune!");
        Managers.Deck.AddRune(this);
    }

}
