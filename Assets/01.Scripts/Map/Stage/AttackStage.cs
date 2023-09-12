using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStage : Stage
{
    public override void InStage()
    {
        base.InStage();

        Enemy enemy = Managers.Map.CurrentChapter.GetEnemy();
        if(enemy.enemyName == "기생충")
        {
            Enemy virtualEnemy = Managers.Map.CurrentChapter.GetEnemy();
            virtualEnemy.isEnter = false;
            Managers.Enemy.AddEnemy(virtualEnemy);
        }
        Managers.Enemy.AddEnemy(enemy);
        Managers.Scene.LoadScene(Define.Scene.DialScene);
    }
}
