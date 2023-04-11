using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : BaseRune
{
    public override bool AbilityCondition()
    {
        return true;
    }

    public override void AbilityAction()
    {
        StatusManager.Instance.AddStatus(BattleManager.Instance.enemy, StatusName.Fire, 5);
    }
}
