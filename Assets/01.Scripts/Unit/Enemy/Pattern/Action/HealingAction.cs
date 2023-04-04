using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingAction : PatternAction
{
    [SerializeField] protected int _value;

    public override void TakeAction()
    {
        BattleManager.Instance.enemy.AddHP(_value);
    }
}
