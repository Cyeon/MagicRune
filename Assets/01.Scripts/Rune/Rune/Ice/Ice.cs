using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : BaseRune
{
    public override bool AbilityCondition()
    {
        return true;
    }

    public override void AbilityAction()
    {
        StatusManager.Instance.AddStatus(BattleManager.Instance.enemy, StatusName.Ice, 3);
    }
}
