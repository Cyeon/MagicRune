using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBullet : BaseRune
{
    public override void AbilityAction()
    {
        Managers.GetPlayer().Attack(GetAbliltiValaue(EffectType.Attack));
    }
}
