using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum RuneSelectMode
{
    None, // Default
    Delete, // 선택 룬 삭제
    Copy, // 선택 룬 복제 
    Enforce // 선택 룬 강화 
}
/// <summary>
/// Scroll View안에 들어갈 개벌 룬 Panel에 붙어 있는 스크립트
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
        _runeDescText.SetText(_baseRune.BaseRuneSO.RuneDescription());
        _runeCoolTimeText.SetText(_baseRune.BaseRuneSO.CoolTime.ToString());
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (_selectMode) // 모드에 따라 다른 기능 해줌 
        {
            case RuneSelectMode.Delete:
                Managers.Deck.RemoveDeck(_baseRune);
                break;
            case RuneSelectMode.Copy:
                Managers.Deck.AddRune(Managers.Rune.GetRune(_baseRune));
                break;
            case RuneSelectMode.Enforce:
                //강화하는 거 생기면 그거 함수 넣어주면 됨  
                break;
            case RuneSelectMode.None:
            default:
                break;
        }

        EventManager<BaseRune>.TriggerEvent(Define.SELECT_RUNE_EVENT, _baseRune); // RuneEventUI 쪽에서 UI 처리 해줌 
    }
}