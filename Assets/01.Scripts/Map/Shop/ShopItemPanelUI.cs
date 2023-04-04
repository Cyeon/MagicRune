using System.Collections;
using System.Collections.Generic;
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
    private Image icon;
    private TextMeshProUGUI goldText;
    private ShopItemSO item;
    public TextMeshProUGUI userGold;

    public void Init(ShopItemSO item)
    {
        icon.sprite = item.icon;
        goldText.SetText(item.gold.ToString());
        this.item = item;
    }

    public void Buy()
    {
        if (item.CheckAvailability())
        {
            item.Buy();

            RuneItem rune = item as RuneItem;
            DeckManager.Instance.AddRune(new Rune(rune.rune));
            userGold.SetText(GameManager.Instance.Gold.ToString());
            gameObject.SetActive(false);
        }
    }

    public void Awake()
    {
        icon = transform.Find("Button/Icon").GetComponent<Image>();
        goldText = transform.GetComponentInChildren<TextMeshProUGUI>();
    }
}
