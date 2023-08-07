using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondBody : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Addressable.Load<BaseRuneSO>("SO/Rune/Ground/" + typeof(DiamondBody).Name);
        base.Init();
    }

    public override void AbilityAction()
    {
        BattleManager.Instance.Player.StatusManager.AddStatus(StatusName.DiamondBody, 1);
    }

    public override object Clone()
    {
        DiamondBody diamondBody = new DiamondBody();
        diamondBody.Init();
        diamondBody.UnEnhance();
        return diamondBody;
    }
}
