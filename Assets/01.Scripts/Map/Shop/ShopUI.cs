using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ShopUI : MonoBehaviour
{
    private Transform _storeShelf;
    private ShopItemPanelUI _shopItemPanel;
    [SerializeField] private GameObject _buyCheck;

    private void Start()
    {
        _storeShelf = transform.Find("StoreShelf");

        Managers.Canvas.GetCanvas("Shop").enabled = false;
    }

    public void Exit()
    {
        Managers.Map.NextStage();
        Managers.Canvas.GetCanvas("Shop").enabled = false;
        Managers.Canvas.GetCanvas("MapUI").enabled = true;
    }


    public void Open()
    {
        for (int i = _storeShelf.transform.childCount - 1; i >= 0; --i)
        {
            Managers.Resource.Destroy(_storeShelf.transform.GetChild(i).gameObject);
        }
    }

    public void RuneItemProduct(Item item)
    {
        ShopItemPanelUI ui = Managers.Resource.Instantiate("ItemPanel", _storeShelf).GetComponent<ShopItemPanelUI>();
        ui.Init(item, BuyCheck);
    }

    private void BuyCheck(ShopItemPanelUI shopItem)
    {
        _shopItemPanel = shopItem;
        _buyCheck.SetActive(true);
    }

    public void Buy()
    {
        _buyCheck.SetActive(false);

        Managers.Gold.AddGold(-_shopItemPanel.item.Gold);
        _shopItemPanel.item.Execute();

        Managers.Resource.Destroy(_shopItemPanel.gameObject);

        _storeShelf.transform.GetComponentsInChildren<ShopItemPanelUI>().ForEach(x => x.GoldTextColorUpdate());
    }
}