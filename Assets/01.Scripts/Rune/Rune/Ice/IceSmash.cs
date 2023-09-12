using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class IceSmash : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Addressable.Load<BaseRuneSO>("SO/Rune/Ice/" + typeof(IceSmash).Name);
        base.Init();
    }

    public override bool AbilityCondition()
    {
        return BattleManager.Instance.Enemy.StatusManager.GetStatusValue(StatusName.Chilliness) >= 5;
    }

    public override void AbilityAction()
    {
        BattleManager.Instance.Enemy.StatusManager.RemoveStatus(StatusName.Chilliness, 5);
        Managers.GetPlayer().Attack(GetAbliltiValue(EffectType.Attack), IsIncludeKeyword(KeywordName.Penetration));
    }

    public override object Clone()
    {
        IceSmash iceSmash = new IceSmash();
        iceSmash.Init();
        iceSmash.UnEnhance();
        return iceSmash;
    }
}
