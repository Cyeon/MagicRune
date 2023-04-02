using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum EnhanceType
{
    Change, // 변경
    Sacrifice, // 재물
    Grant, // 부여
    COUNT,
}

public class RestPortal : Portal
{
    private RestUI _restUI;

    public override void Init()
    {

    }

    public override void Execute()
    {
        _restUI = CanvasManager.Instance.GetCanvas("Rest").GetComponent<RestUI>();

        CanvasManager.Instance.GetCanvas("MapUI").enabled = false;
        CanvasManager.Instance.GetCanvas("Rest").enabled = true;
        _restUI.PortalStartAnimation();
    }
}
