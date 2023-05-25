using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapidFire : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Resource.Load<BaseRuneSO>("SO/Rune/Fire/" + typeof(RapidFire).Name);
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
        return fire;
    }
}
