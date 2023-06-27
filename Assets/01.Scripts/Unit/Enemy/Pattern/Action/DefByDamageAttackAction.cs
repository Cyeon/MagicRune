using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefByDamageAttackAction : AttackAction
{
    public override void StartAction()
    {
        Enemy.PatternManager.CurrentPattern.desc = Mathf.FloorToInt(Enemy.Shield).ToString();
        Enemy.PatternManager.UpdatePatternUI();

        base.StartAction();
    }

    public override void TurnAction()
    {
        damage = Mathf.FloorToInt(Enemy.Shield);
        count = 1;

        base.TurnAction();
    }
}
