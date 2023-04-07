using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPortal : Portal
{
    private ShopUI _shopUI;
    public List<ShopItemSO> shopItemList = new List<ShopItemSO>();

    public override void Execute()
    {
        Managers.Canvas.GetCanvas("MapUI").enabled = false;
        Managers.Canvas.GetCanvas("Shop").enabled = true;

        if (_shopUI == null)
            _shopUI = Managers.Canvas.GetCanvas("Shop").GetComponent<ShopUI>();

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
            _shopUI.ShopItemProduct(shopItemList[numberList[randomIndex]]);
            numberList.RemoveAt(randomIndex);
        }
        base.Execute();
    }

    public override void Init(Vector2 pos)
    {
        _shopUI = Managers.Canvas.GetCanvas("Shop").GetComponent<ShopUI>();
        base.Init(pos);
    }
}
