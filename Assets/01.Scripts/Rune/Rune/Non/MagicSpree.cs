using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSpree : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Resource.Load<BaseRuneSO>("SO/Rune/Non/" + typeof(MagicSpree).Name);
        base.Init();
    }

    public override void AbilityAction()
    {
        Managers.GetPlayer().Attack(GetAbliltiValue(EffectType.Attack), false);
    }

    public override object Clone()
    {
        MagicSpree magicSpreee = new MagicSpree();
        magicSpreee.Init();
        magicSpreee.UnEnhance();
        return magicSpreee;
    }
}
