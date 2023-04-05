using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventurePortal : Portal
{
    [SerializeField]
    private List<AdventureSO> adventureList = new List<AdventureSO>();

    public override void Execute()
    {
        CanvasManager.Instance.GetCanvas("Adventure").enabled = true;
        CanvasManager.Instance.GetCanvas("Adventure").GetComponent<AdventureUI>().Init(GetAdventure(), this);
    }

    public override void Init(Vector2 pos)
    {

    }

    private AdventureSO GetAdventure()
    {
        if (adventureList.Count == 0) return null;

        int cnt = adventureList.Count;
        return adventureList[Random.Range(0, cnt)];
    }
}
