using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicShield : BaseRune
{
    public override void AbilityAction()
    {
        Managers.GetPlayer().AddShield(GetAbliltiValaue(EffectType.Defence));
    }

    public override void Execute()
    {
        Debug.Log("Buy MagicShield Rune!");
        Managers.Deck.AddRune(this);
    }
}
