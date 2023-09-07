using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PatternPopup : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    private bool _isPoup = false;
    public bool IsPopup => _isPoup;
    [SerializeField] private GameObject _patternPopupObj;

    private TextMeshProUGUI _patternNameText;
    private TextMeshProUGUI _patternDescText;

    private RuneListViewUI _runeListViewUI;

    private void Awake()
    {
        _patternNameText = _patternPopupObj.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        _patternDescText = _patternPopupObj.transform.Find("DescText").GetComponent<TextMeshProUGUI>();

        _runeListViewUI = Managers.Canvas.GetCanvas("Popup").transform.GetComponentInChildren<RuneListViewUI>();
    }

    private void Start()
    {
        _patternPopupObj.SetActive(false);
    }

    public void Popup(bool active)
    {
        _isPoup = active;
        _patternPopupObj.SetActive(_isPoup);

        Pattern pattern = BattleManager.Instance.Enemy.PatternManager.CurrentPattern;
        if (_isPoup && pattern != null)
        {
            pattern.DescriptionInit();
            _patternNameText.text = "<color=#F9B41F>" + pattern.patternName + "</color>";
            _patternDescText.text = pattern.PatternDescription;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Popup(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_runeListViewUI.GetUIActive()) return;
        Popup(true);
    }
}
