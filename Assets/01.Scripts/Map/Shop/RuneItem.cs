using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Rune Item", menuName = "Map/Shop/Rune Item SO")]
public class RuneItem : ShopItemSO
{
    public RuneSO rune;
    public override void Execute()
    {
        Debug.Log(rune.Name);
    }
}
