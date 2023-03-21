using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    private TextMeshProUGUI _goldText;
    private Transform _storeShelf;

    private void Awake()
    {
        _goldText = transform.Find("GoldBar/Amount").GetComponent<TextMeshProUGUI>();
        _storeShelf = transform.Find("StoreShelf");
    }
    
    public void Exit()
    {
        MapManager.Instance.NextStage();
        gameObject.SetActive(false);
    }

    public void Open()
    {
        gameObject.SetActive(true);
        _goldText.SetText(GameManager.Instance.Gold.ToString());
        for (int i = _storeShelf.transform.childCount - 1; i >= 0; --i)
        {
            Destroy(_storeShelf.transform.GetChild(i).gameObject);
        }
    }

    public void ShopItemProduct(ShopItemSO item)
    {
        ShopItemPanelUI ui = Instantiate(item.panelPrefab, _storeShelf);
        ui.Init(item);
        ui.userGold = _goldText;
    }
}
