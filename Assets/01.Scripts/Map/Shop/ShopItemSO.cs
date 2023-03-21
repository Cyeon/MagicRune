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
    public ShopItemType itemType = ShopItemType.Rune;
    public ShopItemPanelUI panelPrefab;

    /// <summary>
    /// 현재 구매가 가능한가?
    /// </summary>
    /// <returns></returns>
    public bool CheckAvailability()
    {
        if (gold > GameManager.Instance.Gold) return false;
        return true;
    }

    /// <summary>
    /// 구매 함수
    /// </summary>
    public void Buy()
    {
        GameManager.Instance.Gold -= gold;
        Execute();
    }

    /// <summary>
    /// 구매시 발동되는 함수
    /// </summary>
    public virtual void Execute() { }
}
