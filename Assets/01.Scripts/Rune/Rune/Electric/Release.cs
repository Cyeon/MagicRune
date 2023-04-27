using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Release : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Resource.Load<BaseRuneSO>("SO/Rune/Electric/" + typeof(Release).Name);
    }

    public override void AbilityAction()
    {
        float dmg = Managers.GetPlayer().StatusManager.GetStatusValue(StatusName.Recharging) * 3;
        Managers.GetPlayer().StatusManager.DeleteStatus(StatusName.Recharging);
        Managers.GetPlayer().Attack(dmg);
    }

    public override object Clone()
    {
        Release release = new Release();
        release.Init();
        return release;
    }
}
