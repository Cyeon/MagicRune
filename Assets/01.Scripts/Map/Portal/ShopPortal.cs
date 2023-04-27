using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPortal : Portal
{
    private ShopUI _shopUI;

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

        List<BaseRune> list = Managers.Rune.GetRandomRune(4);
        for (int i = 0; i < list.Count; i++)
        {
            list[i].SetRandomGold(50, 100);
            _shopUI.RuneItemProduct(list[i]);
        }
        base.Execute();
    }

    public override void Init(Vector2 pos)
    {
        _shopUI = Managers.Canvas.GetCanvas("Shop").GetComponent<ShopUI>();
        base.Init(pos);
    }
}
