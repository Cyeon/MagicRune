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
        UIManager.Instance.Bind<TextMeshProUGUI>("Shop GoldBar Amount", CanvasManager.Instance.GetCanvas("Shop").gameObject);
        _goldText = UIManager.Instance.Get<TextMeshProUGUI>("Shop GoldBar Amount");
        _storeShelf = transform.Find("StoreShelf");

        CanvasManager.Instance.GetCanvas("Shop").enabled = false;
    }
    
    public void Exit()
    {
        MapManager.Instance.NextStage();
        CanvasManager.Instance.GetCanvas("Shop").enabled = false;
        CanvasManager.Instance.GetCanvas("MapUI").enabled = true;
    }


    public void Open()
    {
        _goldText.SetText(GameManager.Instance.Gold.ToString());
        for (int i = _storeShelf.transform.childCount - 1; i >= 0; --i)
        {
            ResourceManager.Instance.Destroy(_storeShelf.transform.GetChild(i).gameObject);
        }
    }

    public void ShopItemProduct(ShopItemSO item)
    {
        ShopItemPanelUI ui = ResourceManager.Instance.Instantiate(item.panelPrefab.name, _storeShelf).GetComponent<ShopItemPanelUI>();
        ui.Init(item);
        ui.userGold = _goldText;
    }
}
