using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStage : Stage
{
    public override void Init()
    {
        base.Init();
    }

    public override void InStage()
    {
        Managers.Enemy.AddEnemy(Resources.Load<Enemy>("Prefabs/Enemy/Ice/Ice_Turtle"));
        Managers.Scene.LoadScene("DialTutorialScene");
    }
}
