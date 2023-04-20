using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBreath : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Resource.Load<BaseRuneSO>("SO/Rune/Fire/" + typeof(FireBreath).Name);
    }

    public override void AbilityAction()
    {
        for(int i = 0; i < 3; i++)
        {
            BattleManager.Instance.Enemy.StatusManager.AddStatus(StatusName.Fire, 3);
        }
    }
}
