using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeDescChangeAction : AttackAction
{
    SnakeScale snake;

    public override void StartAction()
    {
        base.StartAction();

        if(snake == null )
        {
            snake = Enemy.PatternManager.passive as SnakeScale;
        }

        Enemy.PatternManager.CurrentPattern.patternValue = (damage + (damage * (snake.IncreasePercent / 100))).ToString();
        Enemy.PatternManager.UpdatePatternUI();
    }
}
