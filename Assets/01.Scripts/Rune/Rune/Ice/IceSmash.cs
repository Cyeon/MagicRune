using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class IceSmash : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Resource.Load<BaseRuneSO>("SO/Rune/Ice/" + typeof(IceSmash).Name);
    }

    public override bool AbilityCondition()
    {
        return BattleManager.Instance.Enemy.StatusManager.GetStatus(StatusName.Ice) != null;
    }

    public override void AbilityAction()
    {
        BattleManager.Instance.Enemy.StatusManager.DeleteStatus(StatusName.Ice);
        Managers.GetPlayer().Attack(GetAbliltiValaue(EffectType.Attack),IsIncludeKeyword(KeywordType.Penetration));
    }

    public override object Clone()
    {
        IceSmash iceSmash = new IceSmash();
        iceSmash.Init();
        return iceSmash;
    }
}
