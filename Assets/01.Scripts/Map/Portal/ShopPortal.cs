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
        //foreach(ShopItemSO item in shopItemList)
        //{
        //    _shopUI.ShopItemProduct(item);
        //}

        List<int> numberList = new List<int>();
        for(int i = 0; i < shopItemList.Count; i++)
        {
            numberList.Add(i);
        }

        for(int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, numberList.Count);
            _shopUI.ShopItemProduct(shopItemList[randomIndex]);
            numberList.RemoveAt(randomIndex);
        }
    }

    public override void Init()
    {
        _shopUI = CanvasManager.Instance.GetCanvas("Shop").GetComponent<ShopUI>();
    }
}
