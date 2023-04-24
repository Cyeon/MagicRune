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

    // �ӽ÷�. ���߿� ���� ���� ������ ������ ����
    public int MinGold;
    public int MaxGold;

    public int gold;

    public Sprite icon;
    public ShopItemType itemType = ShopItemType.Rune;
    //public ShopItemPanelUI panelPrefab;

    /// <summary>
    /// ���� ���Ű� �����Ѱ�?
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
    /// ���� �Լ�
    /// </summary>
    public void Buy()
    {
        Managers.Gold.AddGold(-gold);
        Execute();
    }

    /// <summary>
    /// ���Ž� �ߵ��Ǵ� �Լ�
    /// </summary>
    public virtual void Execute() { }
}
