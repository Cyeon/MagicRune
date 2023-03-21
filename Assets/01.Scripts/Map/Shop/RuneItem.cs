using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Rune Item", menuName = "Map/Shop/Rune Item SO")]
public class RuneItem : ShopItemSO
{
    public CardSO card;
    public override void Execute()
    {
        Debug.Log(card.MainRune.Name);
    }
}
