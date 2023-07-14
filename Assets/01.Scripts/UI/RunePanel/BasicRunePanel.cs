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

    public virtual void SetUI(BaseRuneSO runeSO, bool isEnhance = false)
    {
        if (runeSO == null)
        {
            _rune = null;
            _nameText.SetText("");
            _coolTimeText.SetText("");
            _descText.SetText("");

            _runeIcon.enabled = false;
            _rankIcon.enabled = false;
            return;
        }

        _runeIcon.enabled = true;

        //_rune = rune;

        if (isEnhance == false)
        {
            _nameText.color = Color.white;
            _nameText.SetText(runeSO.RuneName);
        }
        else
        {
            _nameText.color = Color.green;
            _nameText.SetText(runeSO.RuneName + "+");
        }

        _runeIcon.enabled = true;
        _runeIcon.sprite = runeSO.RuneSprite;
        _coolTimeText.SetText(runeSO.CoolTime.ToString());
        _descText.SetText(runeSO.RuneDescription(isEnhance));

        if (_rankIcon != null)
        {
            _rankIcon.enabled = true;
            _rankIcon.sprite = Resources.Load<Sprite>("Sprite/RankIcon/" + runeSO.Rarity.ToString());
        }
    }
    public virtual void SetRune(BaseRune rune)
    {
        _rune = rune;
    }
}
