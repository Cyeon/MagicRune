using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class ThreeAttack : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Addressable.Load<BaseRuneSO>("SO/Rune/Ground/" + typeof(ThreeAttack).Name);
        base.Init();
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
