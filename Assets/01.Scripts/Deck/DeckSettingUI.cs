using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DeckType
{
    FirstDialDeck,
    OwnDeck,
    Unknown
}
public class DeckSettingUI : MonoBehaviour
{
    private List<DeckRunePanel> _runePanelList = new List<DeckRunePanel>(35); // ���� �� UI ������Ʈ�� �پ��ִ� ��ũ��Ʈ 

    private DeckFollowObject _followObject = null; // ���콺 ����ٴϴ� ������Ʈ 

    private GameObject _backgroundPanel = null; // ������ ������ Panel

    [SerializeField]
    private GameObject _runePanelPrefab = null; // Deck_Rune Prefab �־��ָ� ��  
    [SerializeField]
    private Transform _ownDeckContentTransform = null; // OwnRunePanel ���� Content �־��ֱ� 
    [SerializeField]
    private Transform _dialDeckContentTransform = null; // FirstDialRunePanel ���� Content
    [SerializeField]
    private Button exitButton = null; // Button�̶� ������ X ��ư �־��ֱ� 

    private DeckRunePanel _targetRune = null;
    public DeckRunePanel TargetRune => _targetRune;

    private DeckRunePanel _selectRune = null;
    public DeckRunePanel SelectRune => _selectRune;

    private void Awake()
    {
        if (_runePanelPrefab != null) // ������Ʈ�� �̸� ���� 
        {
            for (int i = 0; i < 30; i++)
            {
                GameObject game = Instantiate(_runePanelPrefab, transform);
                game.transform.SetParent(transform);
                DeckRunePanel runePanel = game.GetComponent<DeckRunePanel>();
                _runePanelList.Add(runePanel);
                //runePanel.enabled = false;
                game.SetActive(false);
            }
        }
        _backgroundPanel = transform.Find("BackgroundPanel").gameObject;

        exitButton.onClick.AddListener(() => ReturnPanels());
        exitButton.onClick.AddListener(() => _backgroundPanel.SetActive(false));
    }
    private void Start()
    {
        // ���콺 Ŀ�� ����ٴ� ������Ʈ ���� 
        {
            GameObject game = Instantiate(_runePanelPrefab, transform);
            Destroy(game.GetComponent<DeckRunePanel>());
            _followObject = game.AddComponent<DeckFollowObject>();
            _followObject.SetCanvasTrasform(GetComponent<RectTransform>());
            game.SetActive(false);
        }
    }

    /// <summary>
    /// �� UI ���ִ� ��ư ��� ��������ָ� �� 
    /// </summary>
    public void ActiveUI()
    {
        SettingAllDeck();
        _backgroundPanel.SetActive(true);
    }

    private void SettingAllDeck()
    {
        SetOwnDeck();
        SetDialDeck();
    }

    /// <summary>
    /// �����ϰ� �ִ� ����� �ҷ��ͼ� UI�� �����
    /// </summary>
    private void SetOwnDeck()
    {
        foreach (Rune rune in DeckManager.Instance.Deck)
        {
            DeckRunePanel runePanel = GetEmptyPanel();
            if (runePanel.enabled == false) { runePanel.enabled = true; }
            if (runePanel == null)
            {
                GameObject game = Instantiate(_runePanelPrefab, transform);
                runePanel = game.GetComponent<DeckRunePanel>();
                _runePanelList.Add(runePanel);
            }
            runePanel.gameObject.SetActive(true);
            runePanel.transform.SetParent(_ownDeckContentTransform);
            runePanel.SetDeck(DeckType.OwnDeck);
            runePanel.Setting(rune);
        }
    }

    /// <summary>
    /// FirstDialDeck�� �ִ� ����� UI�� �����
    /// </summary>
    private void SetDialDeck()
    {
        foreach (Rune rune in DeckManager.Instance.FirstDialDeck)
        {
            DeckRunePanel runePanel = GetEmptyPanel();
            if (runePanel.enabled == false) { runePanel.enabled = true; }

            if (runePanel == null)
            {
                GameObject game = Instantiate(_runePanelPrefab, transform);
                runePanel = game.GetComponent<DeckRunePanel>();
                _runePanelList.Add(runePanel);
            }

            runePanel.gameObject.SetActive(true);
            runePanel.transform.SetParent(_dialDeckContentTransform);
            runePanel.SetDeck(DeckType.FirstDialDeck);
            runePanel.Setting(rune);
        }
    }

    /// <summary>
    /// �̸� �����ص� ���� RunePanel �ҷ���
    /// </summary>
    /// <returns>��� ������ ���� Panel ����</returns>
    private DeckRunePanel GetEmptyPanel()
    {
        return _runePanelList.Find(x => x.IsUse == false && x.gameObject.activeSelf == false);
    }

    /// <summary>
    /// �� Panel �ǵ����� �Լ�
    /// UI ���� �� �� ������� 
    /// </summary>
    private void ReturnPanels()
    {
        List<DeckRunePanel> deckRunePanels = _runePanelList.FindAll(x => x.gameObject.activeSelf == true);
        foreach (DeckRunePanel item in deckRunePanels)
        {
            item.transform.SetParent(this.transform);
            item.gameObject.SetActive(false);
        }
    }

    public void SetSelectRune(DeckRunePanel rune)
    {
        _selectRune = rune;
        if (rune != null)
        {
            _followObject.SetImage(_selectRune.Rune.GetRune().RuneImage);
            _followObject.FollowMouse();
            _followObject.gameObject.SetActive(true);
        }
        else
        {
            _followObject.gameObject.SetActive(false);
        }
    }

    public void SetTargetRune(DeckRunePanel rune)
    {
        _targetRune = rune;
    }

    /// <summary>
    /// ���� Panel�� ������ ���� �� 
    /// </summary>
    /// <param name="type"></param>
    public void Equip(DeckType type)
    {
        if (type != SelectRune.NowDeck)
        {
            switch (type)
            {
                case DeckType.FirstDialDeck:
                    if (DeckManager.Instance.FirstDialDeck.Count < DeckManager.FIRST_DIAL_DECK_MAX_COUNT)
                    {
                        DeckManager.Instance.SetFirstDeck(SelectRune.Rune);
                        DeckManager.Instance.RemoveRune(SelectRune.Rune);
                        SelectRune.transform.SetParent(_dialDeckContentTransform);
                        SelectRune.SetDeck(DeckType.FirstDialDeck);
                    }
                    break;
                case DeckType.OwnDeck:
                    DeckManager.Instance.AddRune(SelectRune.Rune);
                    DeckManager.Instance.RemoveFirstDeck(SelectRune.Rune);
                    SelectRune.transform.SetParent(_ownDeckContentTransform);
                    SelectRune.SetDeck(DeckType.OwnDeck);
                    break;
                case DeckType.Unknown:
                default:
                    break;
            }
        }

        SetSelectRune(null);
        SetTargetRune(null);
    }

    /// <summary>
    /// �� ���� ��ü�� ��
    /// </summary>
    public void Switch()
    {
        Rune tempRune = _selectRune.Rune;

        if (_selectRune.NowDeck != _targetRune.NowDeck)
        {
            if (_selectRune.NowDeck == DeckType.OwnDeck)
            {
                DeckManager.Instance.RemoveFirstDeck(_targetRune.Rune);
                DeckManager.Instance.RemoveRune(_selectRune.Rune);

                DeckManager.Instance.SetFirstDeck(_selectRune.Rune);
                DeckManager.Instance.AddRune(_targetRune.Rune);
            }
            else if (_selectRune.NowDeck == DeckType.FirstDialDeck)
            {
                DeckManager.Instance.RemoveFirstDeck(_selectRune.Rune);
                DeckManager.Instance.RemoveRune(_targetRune.Rune);

                DeckManager.Instance.SetFirstDeck(_targetRune.Rune);
                DeckManager.Instance.AddRune(_selectRune.Rune);
            }
        }

        _selectRune.Setting(_targetRune.Rune);
        _targetRune.Setting(tempRune);

        SetSelectRune(null);
        SetTargetRune(null);
    }
}