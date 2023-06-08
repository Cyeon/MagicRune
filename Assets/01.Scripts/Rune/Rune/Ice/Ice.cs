using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : BaseRune
{
    public override void Init()
    {
        base.Init();
        _baseRuneSO = Managers.Resource.Load<BaseRuneSO>("SO/Rune/Ice/" + typeof(Ice).Name);
    }
    public override void AbilityAction()
    {
        BattleManager.Instance.Enemy.StatusManager.AddStatus(StatusName.Chilliness, 3);
    }

    public override object Clone()
    {
        Ice ice = new Ice();
        ice.Init();
        ice.UnEnhance();
        return ice;
    }
}
