using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
/// <summary>
/// ���� �鿡 �ٴ� ��ũ��Ʈ
/// </summary>
public class DeckRunePanel : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    private DeckSettingUI _deckSettingUI = null;
    private Rune _rune = null;
    private Image _runeImage = null;
    private bool _isUse = false; // �ش� �ǳ��� ���ǰ� �ִ°� 
    public bool IsUse => _isUse;

    private void Start()
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
        _runeImage.sprite = null;
        _runeImage = null;
    }

    public void Setting(Rune rune)
    {
        _rune = rune;
        _isUse = true;
        _runeImage.sprite = _rune.GetRune().RuneImage;
    }

    public void SetParent(Transform transform)
    {
        this.transform.SetParent(transform);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _deckSettingUI.SetActiveRune(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _deckSettingUI.SetActiveRune(null);
    }
}