using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShopItemType
{
    Rune
}

public class ShopItem : MonoBehaviour
{
    public ShopItemSO info;
    public ShopItemType itemType;
    private bool _isBuy = false;
    public bool IsBuy => _isBuy;

    public void Buy()
    {
        if (info.gold > GameManager.Instance.Gold) return;
        GameManager.Instance.Gold -= info.gold;
        _isBuy = true;
        Execute();
    }

    public virtual void Execute() { }
}