using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsorptionEndAction : PatternAction
{
    public override void TakeAction()
    {
        AbsorptionAction action = BattleManager.Instance.Enemy.PatternManager.CurrentPattern.startPattern[0] as AbsorptionAction; 
        BattleManager.Instance.Enemy.OnGetDamage -= action.AbsorptionDamage;
        base.TakeAction();
    }
}
