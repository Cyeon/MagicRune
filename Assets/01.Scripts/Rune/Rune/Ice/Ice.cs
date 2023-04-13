using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : BaseRune
{
    public override void AbilityAction()
    {
        BattleManager.Instance.enemy.StatusManager.AddStatus(StatusName.Ice, 3);
    }

    public override void Execute()
    {
        Debug.Log("Buy Ice Rune!");
        Managers.Deck.AddRune(this);
    }
}
