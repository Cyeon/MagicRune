using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
/// <summary>
/// 개별 룬에 붙는 스크립트
/// </summary>
public class DeckRunePanel : MonoBehaviour, IDropHandler, IEndDragHandler, IBeginDragHandler, IDragHandler
{
    private DeckSettingUI _deckSettingUI = null;
    private Image _runeImage = null;
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

    /// <summary>
    /// 오브젝트 꺼질 때 초기화 해주는 내용 담겨있음 
    /// </summary>
    private void OnDisable()
    {
        _isUse = false;
        _rune = null;
        _nowDeck = DeckType.Unknown;
        _runeImage.sprite = null;
    }

    public void Setting(BaseRune rune)
    {
        _rune = rune;
        _isUse = true;
        _runeImage.sprite = _rune.BaseRuneSO.RuneImage;
    }


    public void SetDeck(DeckType type)
    {
        _nowDeck = type;
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