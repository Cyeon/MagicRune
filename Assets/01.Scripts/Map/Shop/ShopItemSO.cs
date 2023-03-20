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

    public void Buy()
    {
        if (gold > GameManager.Instance.Gold) return;
        GameManager.Instance.Gold -= gold;
        _isBuy = true;
        Execute();
    }

    public virtual void Execute() { }
}
