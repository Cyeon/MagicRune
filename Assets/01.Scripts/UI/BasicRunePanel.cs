using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BasicRunePanel : MonoBehaviour, IPointerClickHandler
{
    #region UI Parameter
    [SerializeField]
    private TextMeshProUGUI _nameText;
    [SerializeField]
    private Image _runeIcon;
    [SerializeField]
    private TextMeshProUGUI _coolTimeText;
    [SerializeField]
    private TextMeshProUGUI _descText;
    [SerializeField]
    private Image _rankIcon;
    #endregion

    private BaseRune _rune;
    public BaseRune Rune => _rune;

    public Action ClickAction;

    public void OnPointerClick(PointerEventData eventData)
    {
        ClickAction?.Invoke();
    }

    public virtual void SetUI(BaseRune rune, bool isEnhance = false)
    {
        if (rune == null)
        {
            _rune = null;
            _nameText.SetText("");
            _runeIcon.enabled = false;
            _coolTimeText.SetText("");
            _descText.SetText("");
            return;
        }

        _rune = rune;

        if (isEnhance == false)
        {
            _nameText.color = Color.white;
            _nameText.SetText(_rune.BaseRuneSO.RuneName);
        }
        else
        {
            _nameText.color = Color.green;
            _nameText.SetText(_rune.BaseRuneSO.RuneName + "+");
        }

        _runeIcon.enabled = true;
        _runeIcon.sprite = _rune.BaseRuneSO.RuneSprite;
        _coolTimeText.SetText(_rune.BaseRuneSO.CoolTime.ToString());
        _descText.SetText(_rune.BaseRuneSO.RuneDescription(isEnhance));

        if (_rankIcon != null)
        {
            _rankIcon.sprite = Resources.Load<Sprite>("Sprite/RankIcon/" + rune.BaseRuneSO.Rarity.ToString());
        }
    }
}
