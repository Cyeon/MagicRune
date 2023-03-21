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
    /// ���� ���Ű� �����Ѱ�?
    /// </summary>
    /// <returns></returns>
    public bool CheckAvailability()
    {
        if (gold > GameManager.Instance.Gold) return false;
        return true;
    }

    /// <summary>
    /// ���� �Լ�
    /// </summary>
    public void Buy()
    {
        GameManager.Instance.Gold -= gold;
        Execute();
    }

    /// <summary>
    /// ���Ž� �ߵ��Ǵ� �Լ�
    /// </summary>
    public virtual void Execute() { }
}
