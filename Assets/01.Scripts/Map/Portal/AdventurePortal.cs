using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AdventureFunc
{

}

public class AdventurePortal : Portal
{
    [SerializeField]
    private List<AdventureSO> adventureList = new List<AdventureSO>();

    public override void Execute()
    {
        AdventureSO adventure = GetAdventure();
        Debug.Log(adventure.name);
        Debug.Log(adventure.message);
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
