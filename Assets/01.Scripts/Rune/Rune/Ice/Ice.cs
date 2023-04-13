using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : BaseRune
{
    public override void AbilityAction()
    {
        BattleManager.Instance.enemy.StatusManager.AddStatus(StatusName.Chilliness, 3);
    }
}
