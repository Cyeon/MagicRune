using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem<T>
{
    public int Gold;
    public T Item;
}

public class ShopItemPanelUI : MonoBehaviour
{
    private Image icon;
    private TextMeshProUGUI goldText;
    private ShopItemSO item;

    // ���� ���� �ٸ� ������ ���� �ִ� Ŭ����(SO ����)�� ����� ���ߵ�
    public TextMeshProUGUI userGold;

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
            userGold.SetText(GameManager.Instance.Gold.ToString());
            gameObject.SetActive(false);
        }
    }

    public void Awake()
    {
        icon = transform.Find("Icon").GetComponent<Image>();
        goldText = transform.GetComponentInChildren<TextMeshProUGUI>();
    }
}
