using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CoolTimeRunePanel : BasicRuneAddon, IPointerClickHandler
{
    [SerializeField]
    private GameObject _coolTimePanel;
    [SerializeField]
    private TextMeshProUGUI _coolTimeText;

    private PopupKeyword _keyword = null;

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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_keyword == null)
        {
            _keyword = GetComponentInParent<PopupKeyword>();
        }

        _keyword.MoveKeywordArea(transform);
        _keyword.SetKeyword(Basic.Rune.BaseRuneSO);
    }
}
