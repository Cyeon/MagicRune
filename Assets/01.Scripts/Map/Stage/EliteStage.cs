using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteStage : Stage
{
    public override void InStage()
    {
        base.InStage();

        Managers.Enemy.AddEnemy(Managers.Map.CurrentChapter.GetEliteEnemy());
        Managers.Scene.LoadScene(Define.Scene.DialScene);
    }
}
