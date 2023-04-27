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

    // 지금은 이렇게 하고 나중에는 룬이나 유물 같은 애들에 부모 클래스를 정의해서 관리하는 식으로
    public RuneSO Item;
}

public class ShopItemPanelUI : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _goldText;
    [SerializeField] private TextMeshProUGUI _runeNameText;
    [SerializeField] private TextMeshProUGUI _runeDescText;

    private Item _iItem;

    /// <summary>
    /// 아이템을 샀을때의 행동을 이걸로 넘길려했는데 필요없어짐. 혹시 모르니까 남겨둠
    /// </summary>
    private Action _buyAction;

    public void Init(Item item, Action action = null)
    {
        _iItem = item;
        _buyAction = action;

        _icon.sprite = _iItem.Icon;
        _goldText.SetText(_iItem.Gold.ToString());
        _runeNameText.SetText(_iItem.Rune.BaseRuneSO.RuneName);
        _runeDescText.SetText(_iItem.Rune.BaseRuneSO.RuneDescription);

        transform.localScale = Vector3.one * 0.8f;
    }

    public void Buy()
    {
        //  단순 골드 비교면 이거면 충분. 하지만 다른 조건이 붙으면 함수하나 정으해야할 듯
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
            Managers.Resource.Destroy(this.gameObject);
        }
    }
}
