using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : BaseRune
{
    public override void AbilityAction()
    {
        BattleManager.Instance.enemy.StatusManager.AddStatus(StatusName.Fire, 5);
    }
}
