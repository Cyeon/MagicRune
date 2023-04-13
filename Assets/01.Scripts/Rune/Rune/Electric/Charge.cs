using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : BaseRune
{
    public override void AbilityAction()
    {
        Managers.GetPlayer().StatusManager.AddStatus(StatusName.Recharging, 5);
    }

    public override void Execute()
    {
        Debug.Log("Buy Charge Rune!");
        Managers.Deck.AddRune(this);
    }
}
