using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEPatternInit : StatusEvent
{
    public override void Invoke()
    {
        base.Invoke();
        (_unit as Enemy).PatternManager.CurrentPattern.Init();
    }
}
