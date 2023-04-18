using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UserInfoUI : MonoBehaviour
{
    private TextMeshProUGUI _hpText;
    private TextMeshProUGUI _goldText;

    private void Awake()
    {
        _hpText = transform.Find("HP_Image/Text").GetComponent<TextMeshProUGUI>();
        _goldText = transform.Find("Coin_Image/Text").GetComponent<TextMeshProUGUI>();
    }

    public void UpdateHealthText()
    {
        _hpText.text = string.Format("{0} / {1}", Managers.GetPlayer().HP.ToString(), Managers.GetPlayer().MaxHP.ToString());
    }

    public void UpdateGoldText()
    {
        _goldText.text = Managers.Gold.Gold.ToString();
    }
}
