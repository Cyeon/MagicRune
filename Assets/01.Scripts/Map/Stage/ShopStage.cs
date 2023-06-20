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

        for (int i = 0; i < 4; i++)
        {
            BaseRune rune = Managers.Rune.GetRandomRune();
            rune.SetRandomGold(50, 100);
            _shopUI.RuneItemProduct(rune);
        }
    }

    public override void Init()
    {
        _shopUI = Managers.Canvas.GetCanvas("Shop").GetComponent<ShopUI>();
    }
}
