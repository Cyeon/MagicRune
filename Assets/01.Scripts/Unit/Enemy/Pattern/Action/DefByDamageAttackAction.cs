using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefByDamageAttackAction : AttackAction
{
    public override void StartAction()
    {
        damage = Mathf.FloorToInt(Enemy.Shield);
        count = 1;

        Enemy.PatternManager.CurrentPattern.desc = Mathf.FloorToInt(Enemy.Shield).ToString();
        Enemy.PatternManager.UpdatePatternUI();
        Pattern.DescriptionInit();

        base.StartAction();
    }

    public override void TurnAction()
    {
        base.TurnAction();
    }
}
