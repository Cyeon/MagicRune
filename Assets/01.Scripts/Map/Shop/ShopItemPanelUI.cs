using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static MapDefine;

public class ShopItemPanelUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI goldText;
    public ShopItemSO item;

    public void Init(ShopItemSO item)
    {
        icon.sprite = item.icon;
        goldText.SetText(item.gold.ToString());
        this.item = item;
    }

    public void Buy()
    {   
        if(item.CheckAvailability())
        {
            item.Buy();
            MapSceneUI.InfoUIReload();
            gameObject.SetActive(false);
        }
    }

    public void Awake()
    {
        icon = transform.Find("Icon").GetComponent<Image>();
        goldText = transform.GetComponentInChildren<TextMeshProUGUI>();
    }
}
