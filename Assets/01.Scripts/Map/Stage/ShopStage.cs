using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopStage : Stage
{
    private ShopUI _shopUI;

    public override void InStage()
    {
        base.InStage();

        Managers.Canvas.GetCanvas("MapUI").enabled = false;
        Managers.Canvas.GetCanvas("Shop").enabled = true;

        if (_shopUI == null)
            _shopUI = Managers.Canvas.GetCanvas("Shop").GetComponent<ShopUI>();

        _shopUI.Open();

        List<BaseRune> list = Managers.Rune.GetRandomRune(4);
        for (int i = 0; i < list.Count; i++)
        {
            list[i].SetRandomGold(50, 100);
            _shopUI.RuneItemProduct(list[i]);
        }
    }

    public override void Init()
    {
        _shopUI = Managers.Canvas.GetCanvas("Shop").GetComponent<ShopUI>();
    }
}
