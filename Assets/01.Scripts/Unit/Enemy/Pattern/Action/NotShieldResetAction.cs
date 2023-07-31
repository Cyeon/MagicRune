using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotShieldResetAction : PatternAction
{
    public override void StartAction()
    {
        Enemy.ResetShield();
        Enemy.IsShiledReset = false;
        base.StartAction();
    }

    public override void EndAction()
    {
        Enemy.IsShiledReset = true;
        base.EndAction();
    }
}
