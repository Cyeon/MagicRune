using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RestPortal : Portal
{
    private RestUI _restUI;

    public override void Execute()
    {
        _restUI = Managers.Canvas.GetCanvas("Rest").GetComponent<RestUI>();

        Managers.Canvas.GetCanvas("MapUI").enabled = false;
        Managers.Canvas.GetCanvas("Rest").enabled = true;

        if(_restUI != null)
        {
            _restUI.Dial.gameObject.SetActive(true);
        }
        base.Execute();
    }
}
