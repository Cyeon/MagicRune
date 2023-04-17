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

    // 임시로. 나중에 구조 갈아 엎으면 삭제할 예정
    public int MinGold;
    public int MaxGold;

    public int gold;

    public Sprite icon;
    public ShopItemType itemType = ShopItemType.Rune;
    //public ShopItemPanelUI panelPrefab;

    /// <summary>
    /// 현재 구매가 가능한가?
    /// </summary>
    /// <returns></returns>
    public bool CheckAvailability()
    {
        if (gold > Managers.Gold.Gold) return false;
        return true;
    }

    private void OnValidate()
    {
        gold = Random.Range(MinGold, MaxGold + 1);
    }

    /// <summary>
    /// 구매 함수
    /// </summary>
    public void Buy()
    {
        Managers.Gold.AddGold(-gold);
        Execute();
    }

    /// <summary>
    /// 구매시 발동되는 함수
    /// </summary>
    public virtual void Execute() { }
}
