using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieAction : PatternAction
{
    public override void TakeAction()
    {
        BattleManager.Instance.Enemy.Die();
    }
}
