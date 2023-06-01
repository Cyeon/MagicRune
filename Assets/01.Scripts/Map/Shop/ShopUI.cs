using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    private Transform _storeShelf;
    private ExplainPanel _explainPanel;
    private ShopItemPanelUI _selectItem;
    [SerializeField] private GameObject _buyCheck;

    private void Start()
    {
        _storeShelf = transform.Find("StoreShelf");
        _explainPanel = transform.Find("Explain_Panel").GetComponent<ExplainPanel>();

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
        ShopItemPanelUI ui = Managers.Resource.Instantiate("NewItemPanel", _storeShelf).GetComponent<ShopItemPanelUI>();
        ui.Init(item, SelectItem);
    }

    // 선택
    private void SelectItem(ShopItemPanelUI shopItem)
    {
        _selectItem = shopItem;

        _explainPanel.SetUI(_selectItem.item.Rune.BaseRuneSO);
    }

    public void BuyCheck()
    {
        //_shopItemPanel = shopItem;

        if (_selectItem == null) return;

        if(Managers.Gold.Gold < _selectItem.item.Gold)
        {
            InfoMessage message = Managers.Resource.Instantiate("InfoMessage", transform).GetComponent<InfoMessage>();
            message.Setup("돈이 부족합니다.", Input.GetTouch(0).position);
            return;
        }

        _buyCheck.SetActive(true);
    }

    public void Buy()
    {
        _buyCheck.SetActive(false);

        Managers.Gold.AddGold(-_selectItem.item.Gold);
        Managers.Sound.PlaySound("SFX/Buy", SoundType.Effect);
        _selectItem.item.Execute();

        _selectItem.SoldOut();

        _selectItem = null;
        BaseRune nullRune = null;
        _explainPanel.SetUI(nullRune);

        _storeShelf.transform.GetComponentsInChildren<ShopItemPanelUI>().ForEach(x => x.GoldTextColorUpdate());
    }
}