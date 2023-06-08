using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class ThreeAttack : BaseRune
{
    public override void Init()
    {
        base.Init();
        _baseRuneSO = Managers.Resource.Load<BaseRuneSO>("SO/Rune/Ground/" + typeof(ThreeAttack).Name);
    }

    public override void AbilityAction()
    {
        int count = IsEnhanced ? 4 : 3;
        for(int i = 0; i < count; i++)
        {
            BattleManager.Instance.Enemy.StatusManager.AddStatus(StatusName.Impact, GetAbliltiValue(EffectType.Status).RoundToInt());
        }
    }

    public override object Clone()
    {
        ThreeAttack threeAttack = new ThreeAttack();
        threeAttack.Init();
        threeAttack.UnEnhance();
        return threeAttack;
    }
}
