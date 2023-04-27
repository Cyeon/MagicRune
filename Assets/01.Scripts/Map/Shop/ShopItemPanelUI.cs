using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem
{
    public int Gold;
    public ShopItemType Type;

    // ������ �̷��� �ϰ� ���߿��� ���̳� ���� ���� �ֵ鿡 �θ� Ŭ������ �����ؼ� �����ϴ� ������
    public RuneSO Item;
}

public class ShopItemPanelUI : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _goldText;
    [SerializeField] private TextMeshProUGUI _runeNameText;
    [SerializeField] private TextMeshProUGUI _runeDescText;

    public Item item;

    /// <summary>
    /// �������� �������� �ൿ�� �̰ɷ� �ѱ���ߴµ� �ʿ������. Ȥ�� �𸣴ϱ� ���ܵ�
    /// </summary>
    private Action<ShopItemPanelUI> _buyAction;

    public void Init(Item item, Action<ShopItemPanelUI> action = null)
    {
        this.item = item;
        _buyAction = action;

        _icon.sprite = this.item.Icon;
        _goldText.SetText(this.item.Gold.ToString());
        _runeNameText.SetText(this.item.Rune.BaseRuneSO.RuneName);
        _runeDescText.SetText(this.item.Rune.BaseRuneSO.RuneDescription);
        GoldTextColorUpdate();

        transform.localScale = Vector3.one * 0.8f;
    }

    public void BuyCheck()
    {
        //  �ܼ� ��� �񱳸� �̰Ÿ� ���. ������ �ٸ� ������ ������ �Լ��ϳ� �����ؾ��� ��
        if (Managers.Gold.Gold >= item.Gold)
        {
            _buyAction?.Invoke(this);
        }
    }

    public void GoldTextColorUpdate()
    {
        _goldText.color = Managers.Gold.Gold < item.Gold ? Color.red : Color.white;
    }
}
