using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class ThreeAttack : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Resource.Load<BaseRuneSO>("SO/Rune/Ground/" + typeof(Attack).Name);
    }

    public override void AbilityAction()
    {
        for(int i = 0; i < 3; i++)
        {
            BattleManager.Instance.Enemy.StatusManager.AddStatus(StatusName.Impact, GetAbliltiValaue(EffectType.Status).RoundToInt());
        }
    }
}
