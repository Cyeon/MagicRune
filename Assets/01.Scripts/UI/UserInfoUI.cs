using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UserInfoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _hpText;
    [SerializeField] private TextMeshProUGUI _goldText;

    public void UpdateHealthText()
    {
        _hpText.text = string.Format("{0} / {1}", Managers.GetPlayer().HP.ToString(), Managers.GetPlayer().MaxHP.ToString());
    }

    public void UpdateGoldText()
    {
        _goldText.text = Managers.Gold.Gold.ToString();
    }
}
