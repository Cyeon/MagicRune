using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventureStage : Stage
{
    [SerializeField]
    private List<AdventureSO> _adventureList = new List<AdventureSO>();

    public override void InStage()
    {
        base.InStage();

        Managers.Canvas.GetCanvas("Adventure").enabled = true;
        Managers.Canvas.GetCanvas("Adventure").GetComponent<AdventureUI>().Init(GetAdventure());
    }

    private AdventureSO GetAdventure()
    {
        if (_adventureList.Count == 0) return null;

        int cnt = _adventureList.Count;
        return _adventureList[Random.Range(0, cnt)];
    }
}
