using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageByHealingAction : HealingAction
{
    public override void TakeAction()
    {
        _value = Mathf.FloorToInt(BattleManager.Instance.enemy.currentDmg);
        base.TakeAction();
    }
}
