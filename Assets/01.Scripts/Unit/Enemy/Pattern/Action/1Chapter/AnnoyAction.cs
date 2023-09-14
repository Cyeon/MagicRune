using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnoyAction : PatternAction
{
    [SerializeField] private int annoyMutiple = 5;

    public override void DamageApplyAction()
    {
        int annoyValue = Enemy.StatusManager.GetStatusValue(StatusName.Annoy);
        if (annoyValue > 0)
        {
            Enemy.attackDamage += annoyValue * annoyMutiple;
        }

        base.DamageApplyAction();
    }
}
