using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 유물이나 모험방 이벤트 등으로 룬의 변동이 있을 때 이 스크립트가 붙은 프리팹을 통해 
/// 룬을 삭제/복제/강화 등의 행동을 해줌 (정확힌 UI를 보여줌)
/// </summary>
public class RuneEventUI : MonoBehaviour
{
    private BasicRunePanel _selectedRuneObject = null;
    private GameObject _scrollView = null;
    private Transform _content = null;
    private List<GameObject> _runePanelList = new List<GameObject>();
    private Image _blur;
    private Button _selectButton;
    private Canvas _canvas;

    private void Start()
    {
        _scrollView = transform.Find("Scroll View").gameObject;
        _scrollView.SetActive(false);

        _blur = transform.Find("Blur").GetComponent<Image>();
        _blur.enabled = false;

        _content = _scrollView.GetComponent<ScrollRect>().content;

        _selectButton = transform.Find("SelectButton").GetComponent<Button>();
        _selectButton.onClick.AddListener(() =>
        {
            _scrollView.SetActive(false);
            _blur.enabled = false;

            Sequence seq = DOTween.Sequence();
            seq.Append(_selectedRuneObject.transform.DOScale(1.2f, 0.1f));
            seq.Append(_selectedRuneObject.transform.DOScale(0, 0.2f));
            seq.AppendCallback(() => _selectedRuneObject.gameObject.SetActive(false));

            ReturnRunePanels();
            _selectButton.gameObject.SetActive(false);
            _canvas.enabled = false;
        });
        _selectButton.gameObject.SetActive(false);

        _selectedRuneObject = transform.Find("Basic").GetComponent<BasicRunePanel>();
        _selectedRuneObject.gameObject.SetActive(false);

        _canvas = transform.GetComponent<Canvas>();
        _canvas.enabled = false;
    }

    private void OnEnable()
    {
        EventManager<BaseRune>.StartListening(Define.SELECT_RUNE_EVENT, PopupSelectRune);
        EventManager<RuneSelectMode>.StartListening(Define.RUNE_EVENT_SETTING, SettingRunePanels);
    }

    private void OnDisable()
    {
        EventManager<BaseRune>.StopListening(Define.SELECT_RUNE_EVENT, PopupSelectRune);
        EventManager<RuneSelectMode>.StopListening(Define.RUNE_EVENT_SETTING, SettingRunePanels);
    }

    /// <summary>
    /// 복제/제거 뭐든 일단 하기로 선택한 룬을 크게 띄워줌 
    /// </summary>
    /// <param name="rune"></param>
    private void PopupSelectRune(BaseRune rune)
    {
        _selectedRuneObject.SetUI(rune.BaseRuneSO);
        _selectedRuneObject.gameObject.SetActive(true);
        _selectedRuneObject.transform.localScale = Vector3.one * 1.8f;

        _selectButton.gameObject.SetActive(true);
        _blur.enabled = true;

        Sequence seq = DOTween.Sequence();
        seq.Append(_selectedRuneObject.transform.DOScale(2.2f, 0.2f));
        seq.Append(_selectedRuneObject.transform.DOScale(2f, 0.1f));
    }

    /// <summary>
    /// 덱에 있는 룬들을 선택할 수 있도록 띄워줌
    /// </summary>
    /// <param name="mode">복제/제거 등의 모드 선택</param>
    private void SettingRunePanels(RuneSelectMode mode)
    {
        if (_runePanelList.Count > 0) { ReturnRunePanels(); }
        _canvas.enabled = true;

        foreach (BaseRune rune in Managers.Deck.Deck)
        {
            SelectRunePanel selectPanel = Managers.Resource.Instantiate("UI/RunePanel/Select").GetComponent<SelectRunePanel>();

            if (selectPanel != null)
            {
                selectPanel.SetUI(rune.BaseRuneSO, rune.IsEnhanced);
                selectPanel.SetRune(rune);
                selectPanel.SetMode(mode);
                selectPanel.transform.SetParent(_content);
                selectPanel.transform.localScale = Vector3.one;
                selectPanel.GetComponent<RectTransform>().DOAnchorPos3DZ(-13320, 0);
                _runePanelList.Add(selectPanel.gameObject);
            }
        }

        _scrollView.SetActive(true);
    }

    /// <summary>
    ///  Pool 용도
    /// </summary>
    private void ReturnRunePanels()
    {
        for (int i = 0; i < _runePanelList.Count; i++)
        {
            Managers.Resource.Destroy(_runePanelList[i]);
        }

        _runePanelList.Clear();
    }
}