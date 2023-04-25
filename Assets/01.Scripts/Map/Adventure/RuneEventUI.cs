using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �����̳� ����� �̺�Ʈ ������ ���� ������ ���� �� �� ��ũ��Ʈ�� ���� �������� ���� 
/// ���� ����/����/��ȭ ���� �ൿ�� ���� (��Ȯ�� UI�� ������)
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
            DistracotrFuncList.NextStage(); // ���� ������ �۵���Ű�� �̰� ������ ���� �� ���� ������? �ϴ� �޸� 
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
    /// ����/���� ���� �ϴ� �ϱ�� ������ ���� ũ�� ����� 
    /// </summary>
    /// <param name="rune"></param>
    private void PopupSelectRune(BaseRune rune)
    {
        _runeSpriteImage.sprite = rune.BaseRuneSO.RuneSprite;
        _runeNameText.SetText(rune.BaseRuneSO.RuneName);
        _runeDescText.SetText(rune.BaseRuneSO.RuneDescription);

        _selectedRuneObject.SetActive(true);
    }

    /// <summary>
    /// ���� �ִ� ����� ������ �� �ֵ��� �����
    /// </summary>
    /// <param name="mode">����/���� ���� ��� ����</param>
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

                selectPanel.transform.SetParent(_content);
                _runePanelList.Add(selectPanel.gameObject);
            }
        }

        _scrollView.SetActive(true);
    }

    /// <summary>
    ///  Pool �뵵
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