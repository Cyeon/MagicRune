using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Addressable.Load<BaseRuneSO>("SO/Rune/Ice/" + typeof(Ice).Name);
        base.Init();
    }
    public override void AbilityAction()
    {
        BattleManager.Instance.Enemy.StatusManager.AddStatus(StatusName.Chilliness, GetAbliltiValue(EffectType.Status).RoundToInt());
    }

    public override object Clone()
    {
        Ice ice = new Ice();
        ice.Init();
        ice.UnEnhance();
        return ice;
    }
}
