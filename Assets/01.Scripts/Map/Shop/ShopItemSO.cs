using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShopItemType
{
    Rune
}

public class ShopItemSO : ScriptableObject
{
    public string itemName;
    public int gold;
    public Sprite icon;
    public ShopItemType itemType;
    private bool _isBuy = false;
    public bool IsBuy => _isBuy;

    public bool CheckAvailability()
    {
        if (IsBuy || gold > GameManager.Instance.Gold) return false;
        return true;
    }

    public void Buy()
    {
        GameManager.Instance.Gold -= gold;
        _isBuy = true;
        Execute();
    }

    public virtual void Execute() { }
}
