using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Resource.Load<BaseRuneSO>("SO/Rune/Fire/" + typeof(Fire).Name);
    }

    public override void AbilityAction()
    {
        BattleManager.Instance.Enemy.StatusManager.AddStatus(StatusName.Fire, (int)GetAbliltiValue(EffectType.Status));
    }

    public override object Clone()
    {
        Fire fire = new Fire();
        fire.Init();
        fire.UnEnhance();
        return fire;
    }
}
