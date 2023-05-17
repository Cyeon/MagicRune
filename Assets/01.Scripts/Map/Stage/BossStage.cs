using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStage : Stage
{
    public override void InStage()
    {
        base.InStage();

        Managers.Enemy.AddEnemy(Managers.Map.CurrentChapter.boss);
        Managers.Scene.LoadScene(Define.Scene.DialScene);
    }

    public override void Init()
    {

    }
}
