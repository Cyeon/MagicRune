using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapidFire : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Addressable.Load<BaseRuneSO>("SO/Rune/Fire/" + typeof(RapidFire).Name);
        base.Init();
    }

    public override void AbilityAction()
    {
        for(int i = 0; i < 3; i++)
        {
            BattleManager.Instance.Player.Attack(GetAbliltiValue(EffectType.Attack), false);
        }

        BattleManager.Instance.Player.StatusManager.AddStatus(StatusName.OverHeat, (int)GetAbliltiValue(EffectType.Status));
    }

    public override object Clone()
    {
        RapidFire fire = new RapidFire();
        fire.Init();
        fire.UnEnhance();
        return fire;
    }
}
