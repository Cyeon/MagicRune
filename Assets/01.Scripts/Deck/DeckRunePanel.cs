using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
/// <summary>
/// 개별 룬에 붙는 스크립트
/// </summary>
public class DeckRunePanel : MonoBehaviour, IDropHandler, IEndDragHandler, IBeginDragHandler, IDragHandler
{
    private DeckSettingUI _deckSettingUI = null;

    private GameObject _runeObject = null;
    private Image _runeImage = null;

    private GameObject _cardObject = null;
    private Image _cardImage = null;
    private TextMeshProUGUI _cardNameText = null;
    private TextMeshProUGUI _cardDescText = null;
    private TextMeshProUGUI _cardCoolTimeText = null;

    private BaseRune _rune = null;
    public BaseRune Rune => _rune;

    private DeckType _nowDeck = DeckType.Unknown;
    public DeckType NowDeck => _nowDeck;
    [SerializeField]
    private bool _isUse = false; // 해당 판넬이 사용되고 있는가 
    public bool IsUse => _isUse;

    private void Awake()
    {
        _deckSettingUI = FindObjectOfType<DeckSettingUI>();

        _runeObject = transform.Find("Rune").gameObject;
        _cardObject = transform.Find("Card").gameObject;

        _runeImage = _runeObject.transform.Find("BackSprite").GetChild(0).GetComponent<Image>();
        _cardImage = _cardObject.transform.Find("CardImage").GetComponent<Image>();
        _cardNameText = _cardObject.transform.Find("CardNameText").GetComponent<TextMeshProUGUI>();
        _cardDescText = _cardObject.transform.Find("DescriptionText").GetComponent<TextMeshProUGUI>();
        _cardCoolTimeText = _cardObject.transform.Find("CoolTime_Text").GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// 오브젝트 꺼질 때 초기화 해주는 내용 담겨있음 
    /// </summary>
    private void OnDisable()
    {
        _isUse = false;
        _rune = null;
        _nowDeck = DeckType.Unknown;
        _runeImage.sprite = null;
        _cardImage.sprite = null;
        _cardNameText.SetText("");
        _cardDescText.SetText("");
        _cardCoolTimeText.SetText("");
    }

    public void Setting(BaseRune rune)
    {
        _rune = rune;
        _isUse = true;
        _runeImage.sprite = _rune.BaseRuneSO.RuneSprite;
        _cardImage.sprite = _rune.BaseRuneSO.RuneSprite;
        _cardNameText.SetText(_rune.BaseRuneSO.RuneName);
        _cardDescText.SetText(_rune.BaseRuneSO.RuneDescription);
        _cardCoolTimeText.SetText(_rune.BaseRuneSO.CoolTime.ToString());
    }

    public void SetDeck(DeckType type)
    {
        _nowDeck = type;
        SwitchUIMode(type);
    }

    private void SwitchUIMode(DeckType type)
    {
        switch (type)
        {
            case DeckType.FirstDialDeck:
                _runeObject.SetActive(true);
                _cardObject.SetActive(false);
                break;
            case DeckType.OwnDeck:
                _runeObject.SetActive(false);
                _cardObject.SetActive(true);
                break;
            case DeckType.Unknown:
            default:
                break;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        _deckSettingUI.SetTargetRune(this);
    }
    public void OnDrag(PointerEventData eventData) { } // 이거 없으면 다른 DragHandler 작동 안 해서 지우면 안 됨

    public void OnBeginDrag(PointerEventData eventData)
    {
        _deckSettingUI.SetSelectRune(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_deckSettingUI.SelectRune != null && _deckSettingUI.TargetRune != null)
        {
            _deckSettingUI.Switch();
        }

        _deckSettingUI.SetSelectRune(null);
    }
}