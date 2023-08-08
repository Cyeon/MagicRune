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

        BaseRune[] rune = Managers.Rune.GetRandomRune(6,Managers.Deck.DefaultRune).ToArray();
        for (int i = 0; i < 6; i++)
        {
            rune[i].SetRandomGold(50, 100);
            _shopUI.RuneItemProduct(rune[i]);
        }
    }

    public override void Init()
    {
        _shopUI = Managers.Canvas.GetCanvas("Shop").GetComponent<ShopUI>();
    }
}
