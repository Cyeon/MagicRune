using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPortal : Portal
{
    public List<ShopItemSO> shopItemList = new List<ShopItemSO>();
    public ShopItemSO GetRandomShopItem()
    {
        return shopItemList[Random.Range(0, shopItemList.Count)];
    }

    public override void Execute()
    {

    }

    public override void Init()
    {

    }

    
}
