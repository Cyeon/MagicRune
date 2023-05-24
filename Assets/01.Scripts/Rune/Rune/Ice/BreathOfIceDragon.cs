using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathOfIceDragon : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Resource.Load<BaseRuneSO>("SO/Rune/Ice/" + typeof(BreathOfIceDragon).Name);
    }
    public override void AbilityAction()
    {
        BattleManager.Instance.Enemy.StatusManager.AddStatus(StatusName.Ice, GetAbliltiValue(EffectType.Status).RoundToInt());
    }

    public override object Clone()
    {
        BreathOfIceDragon rune = new BreathOfIceDragon();
        rune.Init();
        return rune;
    }
}
