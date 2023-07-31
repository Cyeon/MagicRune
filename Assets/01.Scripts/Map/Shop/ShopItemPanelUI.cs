using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemPanelUI : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _goldText;
    [SerializeField] private Image _selectImage;
    //[SerializeField] private TextMeshProUGUI _runeNameText;
    //[SerializeField] private TextMeshProUGUI _runeDescText;

    [SerializeField] private GameObject _soldOutPanel;

    // 설명은 밑에 설명 창이 띄워줄 거임

    public Item item;

    /// <summary>
    /// 아이템을 샀을때의 행동을 이걸로 넘길려했는데 필요없어짐. 혹시 모르니까 남겨둠
    /// </summary>
    private Action<ShopItemPanelUI> _buyAction;
    private Action<ShopItemPanelUI> _selectAction;

    public void Init(Item item, Action<ShopItemPanelUI> action = null)
    {
        this.item = item;
        _buyAction = action;

        _icon.sprite = this.item.Rune.Icon;
        _goldText.SetText(this.item.Gold.ToString());
        //_runeNameText.SetText(this.item.Rune.BaseRuneSO.RuneName);
        //_runeDescText.SetText(this.item.Rune.BaseRuneSO.RuneDescription);
        GoldTextColorUpdate();

        transform.localScale = Vector3.one * 0.8f;
        _soldOutPanel.SetActive(false);
    }

    public void BuyCheck()
    {
        //  단순 골드 비교면 이거면 충분. 하지만 다른 조건이 붙으면 함수하나 정으해야할 듯
        _buyAction?.Invoke(this);
    }

    public void GoldTextColorUpdate()
    {
        _goldText.color = Managers.Gold.Gold < item.Gold ? Color.red : Color.white;
    }

    public void SoldOut()
    {
        _soldOutPanel.SetActive(true);
        SetActiveSelectPanel(false);
    }

    public void SetActiveSelectPanel(bool active)
    {
        _selectImage.gameObject.SetActive(active);
    }
}
