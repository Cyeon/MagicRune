using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 유물이나 모험방 이벤트 등으로 룬의 변동이 있을 때 이 스크립트가 붙은 프리팹을 통해 
/// 룬을 삭제/복제/강화 등의 행동을 해줌 (정확힌 UI를 보여줌)
/// </summary>
public class RuneEventUI : MonoBehaviour
{
    #region Selected RunePanel
    private Image _runeSpriteImage = null;
    private TextMeshProUGUI _runeNameText = null;
    private TextMeshProUGUI _runeDescText = null;
    #endregion

    private GameObject _selectedRuneObject = null;
    private GameObject _scrollView = null;
    private Transform _content = null;
    private List<GameObject> _runePanelList = new List<GameObject>();

    [SerializeField]
    private GameObject _runePanelTemplate = null;

    private void Start()
    {
        if (_runePanelTemplate != null)
            Managers.Pool.CreatePool(_runePanelTemplate, 30);

        _scrollView = transform.Find("Scroll View").gameObject;
        _content = _scrollView.GetComponent<ScrollRect>().content;

        Managers.UI.Bind<Image>("SelectedRuneSprite_Image", gameObject);
        Managers.UI.Bind<TextMeshProUGUI>("SelectedRuneName_Text", gameObject);
        Managers.UI.Bind<TextMeshProUGUI>("SelectedRuneDesc_Text", gameObject);

        Managers.UI.Bind<Button>("NextStageButton_Button",gameObject);

        _runeSpriteImage = Managers.UI.Get<Image>("SelectedRuneSprite_Image");
        _runeNameText = Managers.UI.Get<TextMeshProUGUI>("SelectedRuneName_Text");
        _runeDescText = Managers.UI.Get<TextMeshProUGUI>("SelectedRuneDesc_Text");

        _selectedRuneObject = transform.Find("SelectedRune").gameObject;

        Managers.UI.Get<Button>("NextStageButton_Button").onClick.AddListener(() =>
        {
            _scrollView.SetActive(false);
            _selectedRuneObject.SetActive(false);
            ReturnRunePanels();
            //DistracotrFuncList.NextStage(); // 전투 씬에서 작동시키면 이거 때문에 버그 날 수도 있을듯? 일단 메모 
        });

        _scrollView.SetActive(false);
        _selectedRuneObject.SetActive(false);
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
        _runeSpriteImage.sprite = rune.BaseRuneSO.RuneSprite;
        _runeNameText.SetText(rune.BaseRuneSO.RuneName);
        _runeDescText.SetText(rune.BaseRuneSO.RuneDescription());

        _selectedRuneObject.SetActive(true);
    }

    /// <summary>
    /// 덱에 있는 룬들을 선택할 수 있도록 띄워줌
    /// </summary>
    /// <param name="mode">복제/제거 등의 모드 선택</param>
    private void SettingRunePanels(RuneSelectMode mode)
    {
        if (_runePanelList.Count > 0) { ReturnRunePanels(); }

        foreach (BaseRune rune in Managers.Deck.Deck)
        {
            RuneSelectPanelUI selectPanel = Managers.Pool.Pop(_runePanelTemplate).GetComponent<RuneSelectPanelUI>();

            if (selectPanel != null)
            {
                selectPanel.SetMode(mode);
                selectPanel.SetRune(rune);
                selectPanel.SetUI();
                selectPanel.transform.localScale = Vector3.one;

                selectPanel.transform.SetParent(_content);
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
            Managers.Pool.Push(_runePanelList[i].GetComponent<Poolable>());
        }

        _runePanelList.Clear();
    }
}