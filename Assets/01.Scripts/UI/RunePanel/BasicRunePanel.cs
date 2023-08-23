using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.WSA;

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

    [Header("Card Panel Sprite")]
    [SerializeField] private Image _cardPanelImage;
    [SerializeField] private Image _cardCooltimeImage;

    [SerializeField] private Sprite _nonePanel;
    [SerializeField] private Sprite _icePanel;
    [SerializeField] private Sprite _firePanel;
    [SerializeField] private Sprite _groundPanel;
    [SerializeField] private Sprite _electricPanel;

    [SerializeField] private Sprite _noneCoolTime;
    [SerializeField] private Sprite _iceCoolTime;
    [SerializeField] private Sprite _fireCoolTime;
    [SerializeField] private Sprite _groundCoolTime;
    [SerializeField] private Sprite _electricCoolTime;

    public TextMeshProUGUI NameText => _nameText;
    public Image RuneIcon { get { return _runeIcon; } set { _runeIcon = value; } }
    public TextMeshProUGUI CoolTImeText => _coolTimeText;
    public TextMeshProUGUI DescText => _descText;
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
        _descText.SetText(runeSO.RuneDescription(runeSO.KeywardList, isEnhance));

        if (_rankIcon != null)
        {
            _rankIcon.enabled = true;
            _rankIcon.sprite = Resources.Load<Sprite>("Sprite/RankIcon/" + runeSO.Rarity.ToString());
        }

        switch(runeSO.AttributeType)
        {
            case AttributeType.None:
                _cardPanelImage.sprite = _nonePanel;
                _cardCooltimeImage.sprite = _noneCoolTime;
                break;

            case AttributeType.Ice:
                _cardPanelImage.sprite = _icePanel;
                _cardCooltimeImage.sprite = _iceCoolTime;
                break;

            case AttributeType.Fire:
                _cardPanelImage.sprite = _firePanel;
                _cardCooltimeImage.sprite = _fireCoolTime;
                break;

            case AttributeType.Ground:
                _cardPanelImage.sprite = _groundPanel;
                _cardCooltimeImage.sprite = _groundCoolTime;
                break;

            case AttributeType.Electric:
                _cardPanelImage.sprite = _electricPanel;
                _cardCooltimeImage.sprite = _electricCoolTime;
                break;
        }
    }
    public virtual void SetRune(BaseRune rune)
    {
        _rune = rune;
    }
}
