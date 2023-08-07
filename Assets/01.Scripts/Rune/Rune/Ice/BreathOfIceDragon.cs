using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathOfIceDragon : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Addressable.Load<BaseRuneSO>("SO/Rune/Ice/" + typeof(BreathOfIceDragon).Name);
        base.Init();
    }
    public override void AbilityAction()
    {
        BattleManager.Instance.Enemy.StatusManager.AddStatus(StatusName.Ice, GetAbliltiValue(EffectType.Status, StatusName.Ice).RoundToInt());
    }

    public override object Clone()
    {
        BreathOfIceDragon rune = new BreathOfIceDragon();
        rune.Init();
        rune.UnEnhance();
        return rune;
    }
}
