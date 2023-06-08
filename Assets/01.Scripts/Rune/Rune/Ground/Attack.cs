using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Resource.Load<BaseRuneSO>("SO/Rune/Ground/" + typeof(Attack).Name);
        base.Init();
    }

    public override void AbilityAction()
    {
        BattleManager.Instance.Enemy.StatusManager.AddStatus(StatusName.Impact, GetAbliltiValue(EffectType.Status, StatusName.Impact).RoundToInt());
    }

    public override object Clone()
    {
        Attack attack = new Attack();
        attack.Init();
        attack.UnEnhance();
        return attack;
    }
}
