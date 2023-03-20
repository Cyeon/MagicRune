using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Map/Shop/Item SO")]
public class ShopItemSO : ScriptableObject
{
    public string itemName;
    public int gold;
    public Sprite icon;
    public ShopItem item;
}
