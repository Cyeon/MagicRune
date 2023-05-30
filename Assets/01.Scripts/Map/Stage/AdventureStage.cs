using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        List<AdventureSO> enterAdventureList = _adventureList.Where(x => x.isEnter == false).ToList();

        if (enterAdventureList.Count == 0)
        {
            _adventureList.ForEach(x => x.isEnter = false);
            enterAdventureList = _adventureList;
        }

        AdventureSO so = enterAdventureList.GetRandom();
        so.isEnter = true;

        return so;
    }
}
