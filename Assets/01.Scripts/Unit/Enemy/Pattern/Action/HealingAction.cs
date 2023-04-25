using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingAction : PatternAction
{
    [SerializeField] protected int _value;

    public override void TurnAction()
    {
        BattleManager.Instance.Enemy.AddHP(_value);
        base.TurnAction();
    }
}
