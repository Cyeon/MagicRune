using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPortal : Portal
{
    private ShopUI _shopUI;
    public List<ShopItemSO> shopItemList = new List<ShopItemSO>();

    public override void Execute()
    {
        CanvasManager.Instance.GetCanvas("MapUI").enabled = false;
        CanvasManager.Instance.GetCanvas("Shop").enabled = true;

        if (_shopUI == null)
            _shopUI = CanvasManager.Instance.GetCanvas("Shop").GetComponent<ShopUI>();

        _shopUI.Open();
        foreach(ShopItemSO item in shopItemList)
        {
            _shopUI.ShopItemProduct(item);
        }
    }

    public override void Init()
    {
        _shopUI = CanvasManager.Instance.GetCanvas("Shop").GetComponent<ShopUI>();
    }
}
