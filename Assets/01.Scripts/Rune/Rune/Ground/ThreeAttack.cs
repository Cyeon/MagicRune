using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class ThreeAttack : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Resource.Load<BaseRuneSO>("SO/Rune/Ground/" + typeof(ThreeAttack).Name);
    }

    public override void AbilityAction()
    {
        for(int i = 0; i < 3; i++)
        {
            BattleManager.Instance.Enemy.StatusManager.AddStatus(StatusName.Impact, GetAbliltiValue(EffectType.Status).RoundToInt());
        }
    }

    public override object Clone()
    {
        ThreeAttack threeAttack = new ThreeAttack();
        threeAttack.Init();
        return threeAttack;
    }
}
