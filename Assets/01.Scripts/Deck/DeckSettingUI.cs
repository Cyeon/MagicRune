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
    private List<DeckRunePanel> _runePanelList = new List<DeckRunePanel>(35); // 개별 룬 UI 오브젝트에 붙어있는 스크립트 

    private DeckFollowObject _followObject = null; // 마우스 따라다니는 오브젝트 

    private GameObject _backgroundPanel = null; // 반투명 검은색 Panel

    [SerializeField]
    private GameObject _runePanelPrefab = null; // Deck_Rune Prefab 넣어주면 됨  
    [SerializeField]
    private Transform _ownDeckContentTransform = null; // OwnRunePanel 밑의 Content 넣어주기 
    [SerializeField]
    private Transform _dialDeckContentTransform = null; // FirstDialRunePanel 밑의 Content
    [SerializeField]
    private Button exitButton = null; // Button이란 네임의 X 버튼 넣어주기 

    private DeckRunePanel _targetRune = null;
    public DeckRunePanel TargetRune => _targetRune;

    private DeckRunePanel _selectRune = null;
    public DeckRunePanel SelectRune => _selectRune;

    private void Awake()
    {
        if (_runePanelPrefab != null) // 오브젝트들 미리 생성 
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
        // 마우스 커서 따라다닐 오브젝트 생성 
        {
            GameObject game = Instantiate(_runePanelPrefab, transform);
            Destroy(game.GetComponent<DeckRunePanel>());
            _followObject = game.AddComponent<DeckFollowObject>();
            _followObject.SetCanvasTrasform(GetComponent<RectTransform>());
            game.SetActive(false);
        }
    }

    /// <summary>
    /// 덱 UI 켜주는 버튼 등에서 실행시켜주면 됨 
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
    /// 소유하고 있는 룬들을 불러와서 UI에 띄워줌
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
    /// FirstDialDeck에 있는 룬들을 UI에 띄워줌
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
    /// 미리 생성해둔 개별 RunePanel 불러옴
    /// </summary>
    /// <returns>사용 중이지 않은 Panel 리턴</returns>
    private DeckRunePanel GetEmptyPanel()
    {
        return _runePanelList.Find(x => x.IsUse == false && x.gameObject.activeSelf == false);
    }

    /// <summary>
    /// 룬 Panel 되돌리는 함수
    /// UI 닫을 때 꼭 해줘야함 
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
    /// 룬을 Panel에 가져다 뒀을 때 
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
    /// 룬 끼리 교체할 때
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