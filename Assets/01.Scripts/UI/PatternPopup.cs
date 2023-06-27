using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PatternPopup : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    bool _isPoup = false;
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

        if (_isPoup)
        {
            Pattern pattern = BattleManager.Instance.Enemy.PatternManager.CurrentPattern;
            _patternNameText.text = pattern.patternName;
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
