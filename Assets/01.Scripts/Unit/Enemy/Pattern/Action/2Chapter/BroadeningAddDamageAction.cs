using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroadeningAddDamageAction : PatternAction
{
    private Broadening _broadening;

    public override void DamageApplyAction()
    {
        Enemy.attackDamage += _broadening.AddDmg;

        base.DamageApplyAction();
    }
}
