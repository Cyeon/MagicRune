using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestStage : Stage
{
    private RestUI _restUI;

    [SerializeField]
    private AdventureSO rest;

    public override void InStage()
    {
        base.InStage();

        Managers.Canvas.GetCanvas("Adventure").enabled = true;
        Managers.Canvas.GetCanvas("Adventure").GetComponent<AdventureUI>().Init(rest);

        //Managers.Canvas.GetCanvas("MapUI").enabled = false;
        //Managers.Canvas.GetCanvas("Rest").enabled = true;

        if(_restUI == null)
            _restUI = Managers.Canvas.GetCanvas("Rest").GetComponent<RestUI>();
        //_restUI.Dial.gameObject.SetActive(true);
        //_restUI.Dial.ResetDial();
    }

    public override void Init()
    {
        _restUI = Managers.Canvas.GetCanvas("Rest").GetComponent<RestUI>();
    }
}
