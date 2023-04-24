using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Resource.Load<BaseRuneSO>("SO/Rune/Ice/" + typeof(Ice).Name);
    }
    public override void AbilityAction()
    {
        BattleManager.Instance.Enemy.StatusManager.AddStatus(StatusName.Chilliness, 3);
    }
}
