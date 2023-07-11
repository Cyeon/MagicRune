using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestStage : Stage
{
    [SerializeField]
    private AdventureSO rest;

    public override void InStage()
    {
        base.InStage();

        Managers.Canvas.GetCanvas("Adventure").enabled = true;
        Managers.Canvas.GetCanvas("Adventure").GetComponent<AdventureUI>().Init(rest);
    }
}
