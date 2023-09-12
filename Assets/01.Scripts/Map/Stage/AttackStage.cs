using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStage : Stage
{
    public override void InStage()
    {
        base.InStage();

        Managers.Enemy.AddEnemy(Managers.Map.CurrentChapter.GetEnemy();
        Managers.Scene.LoadScene(Define.Scene.DialScene);
    }
}
