using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAttack : BaseRune
{
    public override bool AbilityCondition()
    {
        return true;
    }

    public override void AbilityAction()
    {
        Managers.GetPlayer().Attack(Managers.GetPlayer().GetShield());
    }
}
