using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSmash : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Resource.Load<BaseRuneSO>("SO/Rune/Ice/" + typeof(IceSmash).Name);
    }
    public override void AbilityAction()
    {
        if(BattleManager.Instance.Enemy.StatusManager.GetStatus(StatusName.Ice) != null)
        {
            BattleManager.Instance.Enemy.StatusManager.DeleteStatus(StatusName.Ice);
            Managers.GetPlayer().Attack(GetAbliltiValaue(EffectType.Attack));
        }
    }
}
