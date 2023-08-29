using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStatusAction : PatternAction
{
    public override void TurnAction()
    {
        BattleManager.Instance.Enemy.StatusManager.Reset();
        base.TurnAction();                                                                                                                                                                                                           
    }
}
