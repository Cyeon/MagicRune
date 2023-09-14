using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeAddDamageAction : PatternAction
{
    SnakeScale snake;

    public override void DamageApplyAction()
    {
        if (snake == null)
        {
            snake = Enemy.PatternManager.passive as SnakeScale;
        }

        Enemy.attackDamage += Enemy.attackDamage * (snake.IncreasePercent / 100);

        base.DamageApplyAction();
    }
}
