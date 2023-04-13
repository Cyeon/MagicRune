using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBullet : BaseRune
{
    public override void AbilityAction()
    {
        Managers.GetPlayer().Attack(GetAbliltiValaue(EffectType.Attack));
    }

    public override void Execute()
    {
        Debug.Log("Buy MagicBullet Rune!");
        Managers.Deck.AddRune(this);
    }
}
