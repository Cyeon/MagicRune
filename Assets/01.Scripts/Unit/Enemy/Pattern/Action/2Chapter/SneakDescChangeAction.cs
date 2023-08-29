using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SneakDescChangeAction : PatternAction
{
    SnakeScale snake;

    public override void StartAction()
    {
        base.StartAction();

        if(snake == null )
        {
            Enemy.PatternManager.passive.GetComponent<SnakeScale>();
        }

        Enemy.PatternManager.CurrentPattern.desc = snake.IncreaseDamage.ToString();
        Enemy.PatternManager.UpdatePatternUI();
    }
}
