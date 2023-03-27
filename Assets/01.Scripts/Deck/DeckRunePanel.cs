using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
/// <summary>
/// 개별 룬에 붙는 스크립트
/// </summary>
public class DeckRunePanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDropHandler, IBeginDragHandler
{
    private DeckSettingUI _deckSettingUI = null;
    private Image _runeImage = null;
    private Rune _rune = null;
    public Rune Rune => _rune;

    private DeckType _nowDeck = DeckType.Unknown;
    public DeckType NowDeck => _nowDeck;
    private bool _isUse = false; // 해당 판넬이 사용되고 있는가 
    public bool IsUse => _isUse;

    private void Awake()
    {
        _deckSettingUI = FindObjectOfType<DeckSettingUI>();

        Image[] images = GetComponentsInChildren<Image>();
        foreach (Image item in images)
        {
            if (item.name == "RuneImage")
            {
                _runeImage = item;
                break;
            }
        }
    }
    private void OnDisable()
    {
        _isUse = false;
        _rune = null;
        _nowDeck = DeckType.Unknown;
        _runeImage.sprite = null;
    }

    public void Setting(Rune rune)
    {
        _rune = rune;
        _isUse = true;
        _runeImage.sprite = _rune.GetRune().RuneImage;
    }


    public void SetDeck(DeckType type)
    {
        _nowDeck = type;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _deckSettingUI.SetSelectRune(this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_deckSettingUI.SelectRune != null && _deckSettingUI.TargetRune != null)
        {
            _deckSettingUI.Switch();
        }

        _deckSettingUI.SetSelectRune(null);
    }

    public void OnDrop(PointerEventData eventData)
    {
        _deckSettingUI.SetTargetRune(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _deckSettingUI.SetSelectRune(this);
    }
}