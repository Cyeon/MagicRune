using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : BaseRune
{
    public override void AbilityAction()
    {
        BattleManager.Instance.enemy.StatusManager.AddStatus(StatusName.Fire, 5);
    }

    public override void Execute()
    {
        Debug.Log("Buy Fire Rune!");
        Managers.Deck.AddRune(this);
    }
}
