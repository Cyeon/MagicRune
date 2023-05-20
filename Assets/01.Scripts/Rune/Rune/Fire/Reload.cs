using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reload : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Resource.Load<BaseRuneSO>("SO/Rune/Fire/" + typeof(Reload).Name);
    }

    public override void AbilityAction()
    {
        
    }

    public override object Clone()
    {
        Reload reload = new Reload();
        reload.Init();
        return reload;
    }
}
