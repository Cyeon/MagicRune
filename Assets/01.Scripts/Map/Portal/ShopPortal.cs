using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPortal : Portal
{
    [SerializeField] private ShopUI _shopUI;
    public List<ShopItemSO> shopItemList = new List<ShopItemSO>();

    public override void Execute()
    {
        _shopUI.Open();
        foreach(ShopItemSO item in shopItemList)
        {
            _shopUI.ShopItemProduct(item);
        }
    }

    public override void Init()
    {

    }
}
