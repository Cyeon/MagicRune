using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBullet : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Resource.Load<BaseRuneSO>("SO/Rune/Non/" + typeof(MagicBullet).Name);
    }
    public override void AbilityAction()
    {
        Managers.GetPlayer().Attack(GetAbliltiValaue(EffectType.Attack), IsIncludeKeyword(KeywordType.Penetration));
    }

    public override object Clone()
    {
        MagicBullet magicBullet = new MagicBullet();
        magicBullet.Init();
        return magicBullet;
    }
}
