using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePunch : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Addressable.Load<BaseRuneSO>("SO/Rune/Fire/" + typeof(FirePunch).Name);
        base.Init();
    }
    public override void AbilityAction()
    {
        Managers.GetPlayer().Attack(GetAbliltiValue(EffectType.Attack), IsIncludeKeyword(KeywordName.Penetration));
        BattleManager.Instance.Enemy.StatusManager.AddStatus(StatusName.Fire, 4);
    }

    public override object Clone()
    {
        FirePunch firePunch = new FirePunch();
        firePunch.Init();
        firePunch.UnEnhance();
        return firePunch;
    }
}
