using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopStage : Stage
{
    private ShopUI _shopUI;
    [SerializeField]
    private List<ShopRarityGold> _shopRarityGoldList;

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
            ShopRarityGold rarity = _shopRarityGoldList.Where(x => x.runeRarity == rune[i].BaseRuneSO.Rarity).FirstOrDefault();   
            rune[i].SetRandomGold(rarity.minGold, rarity.maxGold);
            _shopUI.RuneItemProduct(rune[i]);
        }
    }

    public override void Init()
    {
        _shopUI = Managers.Canvas.GetCanvas("Shop").GetComponent<ShopUI>();
    }
}

[Serializable]
public struct ShopRarityGold
{
    public RuneRarity runeRarity;
    public int minGold;
    public int maxGold;
}
