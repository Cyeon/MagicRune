using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum EnhanceType
{
    START,
    Change, // 변경
    Sacrifice, // 재물
    END,
}

public class RestPortal : Portal
{
    private RestUI _restUI;

    public override void Execute()
    {
        _restUI = Managers.Canvas.GetCanvas("Rest").GetComponent<RestUI>();

        Managers.Canvas.GetCanvas("MapUI").enabled = false;
        Managers.Canvas.GetCanvas("Rest").enabled = true;
        _restUI.PortalStartAnimation();
        _restUI.SetRandonEnhanceType(); // 랜덤으로 정해줌
        base.Execute();
    }
}
