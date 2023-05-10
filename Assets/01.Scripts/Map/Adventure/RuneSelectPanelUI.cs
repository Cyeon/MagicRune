using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum RuneSelectMode
{
    None, // Default
    Delete, // ���� �� ����
    Copy, // ���� �� ���� 
    Enforce // ���� �� ��ȭ 
}
/// <summary>
/// Scroll View�ȿ� �� ���� �� Panel�� �پ� �ִ� ��ũ��Ʈ
/// </summary>
public class RuneSelectPanelUI : MonoBehaviour, IPointerClickHandler
{
    private BaseRune _baseRune = null;
    private RuneSelectMode _selectMode = RuneSelectMode.None;

    #region UI

    private Image _runeImage = null;
    private TextMeshProUGUI _runeNameText = null;
    private TextMeshProUGUI _runeDescText = null;
    private TextMeshProUGUI _runeCoolTimeText = null;

    #endregion

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        _runeImage = transform.Find("RuneImage").GetComponent<Image>();
        _runeNameText = transform.Find("RuneNameText").GetComponent<TextMeshProUGUI>();
        _runeDescText = transform.Find("RuneDescText").GetComponent<TextMeshProUGUI>();
        _runeCoolTimeText = transform.Find("RuneCoolTimeText").GetComponent<TextMeshProUGUI>();
    }

    public void SetMode(RuneSelectMode mode)
    {
        _selectMode = mode;
    }

    public void SetRune(BaseRune rune)
    {
        _baseRune = rune;
    }

    public void SetUI()
    {
        if (_runeImage == null || _runeNameText == null || _runeDescText == null || _runeCoolTimeText == null)
        {
            Init();
        }

        _runeImage.sprite = _baseRune.BaseRuneSO.RuneSprite;
        _runeNameText.SetText(_baseRune.BaseRuneSO.RuneName);
        _runeDescText.SetText(_baseRune.BaseRuneSO.RuneDescription);
        _runeCoolTimeText.SetText(_baseRune.BaseRuneSO.CoolTime.ToString());
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (_selectMode) // ��忡 ���� �ٸ� ��� ���� 
        {
            case RuneSelectMode.Delete:
                Managers.Deck.RemoveDeck(_baseRune);
                break;
            case RuneSelectMode.Copy:
                Managers.Deck.AddRune(Managers.Rune.GetRune(_baseRune));
                break;
            case RuneSelectMode.Enforce:
                //��ȭ�ϴ� �� ����� �װ� �Լ� �־��ָ� ��  
                break;
            case RuneSelectMode.None:
            default:
                break;
        }

        EventManager<BaseRune>.TriggerEvent(Define.SELECT_RUNE_EVENT, _baseRune); // RuneEventUI �ʿ��� UI ó�� ���� 
    }
}