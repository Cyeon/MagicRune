using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBullet : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Addressable.Load<BaseRuneSO>("SO/Rune/Non/" + typeof(MagicBullet).Name);
        base.Init();
    }
    public override void AbilityAction()
    {
        Managers.GetPlayer().Attack(GetAbliltiValue(EffectType.Attack), IsIncludeKeyword(KeywordName.Penetration));
    }

    public override object Clone()
    {
        MagicBullet magicBullet = new MagicBullet();
        magicBullet.Init();
        magicBullet.UnEnhance();
        return magicBullet;
    }
}
