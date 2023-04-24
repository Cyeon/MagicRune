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
    private Image _icon;
    private TextMeshProUGUI _goldText;
    private Item _iItem;
    public TextMeshProUGUI userGold;

    /// <summary>
    /// �������� �������� �ൿ�� �̰ɷ� �ѱ���ߴµ� �ʿ������. Ȥ�� �𸣴ϱ� ���ܵ�
    /// </summary>
    private Action _buyAction;

    public void Init(Item item, Action action = null)
    {
        _iItem = item;
        _icon.sprite = _iItem.Icon;
        _goldText.SetText(_iItem.Gold.ToString());
        _buyAction = action;

        transform.localScale = Vector3.one * 0.8f;
    }

    public void Buy()
    {
        //  �ܼ� ��� �񱳸� �̰Ÿ� ���. ������ �ٸ� ������ ������ �Լ��ϳ� �����ؾ��� ��
        if (Managers.Gold.Gold >= _iItem.Gold)
        {
            Managers.Gold.AddGold(-_iItem.Gold);
            _iItem.Execute();

            _buyAction?.Invoke();
            //switch (_iItem.ShopItemType)
            //{
            //    case ShopItemType.Rune:
            //        //Managers.Deck.AddRune(_item as BaseRune);
            //        userGold.SetText(Managers.Gold.Gold.ToString());
            //        gameObject.SetActive(false);
            //        break;
            //}
            userGold.SetText(Managers.Gold.Gold.ToString());
            Managers.Resource.Destroy(this.gameObject);
        }
    }

    public void Awake()
    {
        _icon = transform.Find("Button/Icon").GetComponent<Image>();
        _goldText = transform.GetComponentInChildren<TextMeshProUGUI>();
    }
}
