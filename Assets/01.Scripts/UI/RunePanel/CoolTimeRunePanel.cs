using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoolTimeRunePanel : BasicRuneAddon
{
    [SerializeField]
    private GameObject _coolTimePanel;
    [SerializeField]
    private TextMeshProUGUI _coolTimeText;

    public override void SetUI(BaseRuneSO baseRuneSO, bool isEnhance = true)
    {
        Basic.SetUI(baseRuneSO, isEnhance);

        if (Basic.Rune.IsCoolTime)
        {
            _coolTimePanel.SetActive(true);
            _coolTimeText.SetText(Basic.Rune.CoolTime.ToString());
        }
    }

    public void CoolTimeOff()
    {
        _coolTimePanel.SetActive(false);
    }
}
