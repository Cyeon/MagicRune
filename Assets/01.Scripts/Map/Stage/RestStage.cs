using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestStage : Stage
{
    private RestUI _restUI;

    public override void InStage()
    {
        base.InStage();

        Managers.Canvas.GetCanvas("MapUI").enabled = false;
        Managers.Canvas.GetCanvas("Rest").enabled = true;

        if(_restUI == null)
            _restUI = Managers.Canvas.GetCanvas("Rest").GetComponent<RestUI>();
    }

    public override void Init()
    {
        _restUI = Managers.Canvas.GetCanvas("Rest").GetComponent<RestUI>();
    }
}
