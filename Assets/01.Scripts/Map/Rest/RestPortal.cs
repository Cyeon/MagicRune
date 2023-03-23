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
    #region Rest Parameta
    [SerializeField, Range(0, 100), OverrideLabel("Health %")]
    private int _healthPercent = 25;
    #endregion

    private Button _restBtn;
    private Button _enhanceBtn;

    [SerializeField]
    private RestUI _restUI;

    public override void Init()
    {

    }

    public override void Execute()
    {
        CanvasManager.Instance.GetCanvas("MapUI").enabled = false;
        CanvasManager.Instance.GetCanvas("Rest").enabled = true;
        _restUI.PortalStartAnimation();
    }
}
