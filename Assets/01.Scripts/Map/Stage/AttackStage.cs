using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStage : Stage
{
    private Enemy _stageEnemy;

    public override void InStage()
    {
        base.InStage();

        Managers.Enemy.AddEnemy(_stageEnemy);
        Managers.Scene.LoadScene(Define.Scene.DialScene);
    }

    public void Init(Enemy enemy)
    {
        _stageEnemy = enemy;
    }
}
