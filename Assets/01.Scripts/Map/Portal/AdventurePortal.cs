using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MapDefine;

public class AdventurePortal : Portal
{
    [SerializeField]
    private List<AdventureSO> adventureList = new List<AdventureSO>();

    public override void Execute()
    {
        MapSceneUI.adventureUI.gameObject.SetActive(true);
        MapSceneUI.adventureUI.Init(GetAdventure(), this);
    }

    public override void Init()
    {

    }

    private AdventureSO GetAdventure()
    {
        if (adventureList.Count == 0) return null;

        int cnt = adventureList.Count;
        return adventureList[Random.Range(0, cnt)];
    }
}
