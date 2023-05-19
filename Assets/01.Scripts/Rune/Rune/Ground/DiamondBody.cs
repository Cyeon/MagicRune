using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondBody : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Resource.Load<BaseRuneSO>("SO/Rune/Ground/" + typeof(DiamondBody).Name);
    }

    public override void AbilityAction()
    {
        BattleManager.Instance.Player.StatusManager.AddStatus(StatusName.DiamondBody, 1);
    }

    public override object Clone()
    {
        DiamondBody diamondBody = new DiamondBody();
        diamondBody.Init();
        return diamondBody;
    }
}
