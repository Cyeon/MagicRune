using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAttack : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Resource.Load<BaseRuneSO>("SO/Rune/Ground/" + typeof(ShieldAttack).Name);
    }
    public override void AbilityAction()
    {
        Managers.GetPlayer().Attack(Managers.GetPlayer().GetShield());
    }
}
