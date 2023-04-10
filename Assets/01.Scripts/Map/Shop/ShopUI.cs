using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    private TextMeshProUGUI _goldText;
    private Transform _storeShelf;

    private void Start()
    {
        Managers.UI.Bind<TextMeshProUGUI>("Shop GoldBar Amount", Managers.Canvas.GetCanvas("Shop").gameObject);
        _goldText = Managers.UI.Get<TextMeshProUGUI>("Shop GoldBar Amount");
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
        _goldText.SetText(GameManager.Instance.Gold.ToString());
        for (int i = _storeShelf.transform.childCount - 1; i >= 0; --i)
        {
            Managers.Resource.Destroy(_storeShelf.transform.GetChild(i).gameObject);
        }
    }

    public void ShopItemProduct(ShopItemSO item)
    {
        ShopItemPanelUI ui = Managers.Resource.Instantiate(item.panelPrefab.name, _storeShelf).GetComponent<ShopItemPanelUI>();
        ui.Init(item);
        ui.userGold = _goldText;
    }
}
