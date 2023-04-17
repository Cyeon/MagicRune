using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : BaseRune
{
    public override void AbilityAction()
    {
        BattleManager.Instance.Enemy.StatusManager.AddStatus(StatusName.Fire, 5);
    }
}
