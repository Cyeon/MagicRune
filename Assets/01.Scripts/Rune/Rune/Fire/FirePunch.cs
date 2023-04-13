using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePunch : BaseRune
{
    public override void AbilityAction()
    {
        Managers.GetPlayer().Attack(GetAbliltiValaue(EffectType.Attack));
        BattleManager.Instance.enemy.StatusManager.AddStatus(StatusName.Fire, 4);
    }

    public override void Execute()
    {
        Debug.Log("Buy FirePunch Rune!");
        Managers.Deck.AddRune(this);
    }
}
