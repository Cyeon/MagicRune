using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum DeckType
{
    FirstDialDeck,
    OwnDeck,
    Unknown
}
public class DeckSettingUI : MonoBehaviour
{
    private List<DeckRunePanel> _runePanelList = new List<DeckRunePanel>(35);

    private Dictionary<DeckType, DeckRunePanel> _runeDeckTypeDict = new Dictionary<DeckType, DeckRunePanel>();

    private DeckFollowObject _followObject = null;

    [SerializeField]
    private GameObject _runePanelPrefab = null;
    [SerializeField]
    private Transform _ownDeckContentTransform = null;
    [SerializeField]
    private Transform _dialDeckContentTransform = null;

    private DeckRunePanel _targetRune = null;
    public DeckRunePanel TargetRune => _targetRune;

    private DeckRunePanel _selectRune = null;
    public DeckRunePanel SelectRune => _selectRune;

    private void Awake()
    {
        if (_runePanelPrefab != null)
        {
            for (int i = 0; i < 30; i++)
            {
                GameObject game = Instantiate(_runePanelPrefab, transform);
                game.transform.SetParent(transform.GetChild(0));
                DeckRunePanel runePanel = game.GetComponent<DeckRunePanel>();
                _runePanelList.Add(runePanel);
                runePanel.enabled = false;
                game.SetActive(false);
            }
        }
    }
    private void Start()
    {
        SettingAllDeck();

        {
            GameObject game = Instantiate(_runePanelPrefab, transform);
            Destroy(game.GetComponent<DeckRunePanel>());
            _followObject = game.AddComponent<DeckFollowObject>();
            game.SetActive(false);
        }
    }

    private void SettingAllDeck()
    {
        SetOwnDeck();
        SetDialDeck();
    }

    private void SetOwnDeck()
    {
        foreach (Rune rune in DeckManager.Instance.Deck)
        {
            DeckRunePanel runePanel = GetEmptyPanel();
            if (runePanel.enabled == false) { runePanel.enabled = true; }
            if (runePanel == null)
            {
                GameObject game = Instantiate(_runePanelPrefab, transform);
                _runePanelList.Add(game.GetComponent<DeckRunePanel>());
            }
            runePanel.gameObject.SetActive(true);
            runePanel.Setting(rune);
            runePanel.SetDeck(DeckType.OwnDeck);
            runePanel.transform.SetParent(_ownDeckContentTransform);
        }
    }

    private void SetDialDeck()
    {
        foreach (Rune rune in DeckManager.Instance.FirstDialDeck)
        {
            DeckRunePanel runePanel = GetEmptyPanel();
            if (runePanel.enabled == false) { runePanel.enabled = true; }

            if (runePanel == null)
            {
                GameObject game = Instantiate(_runePanelPrefab, transform);
                _runePanelList.Add(game.GetComponent<DeckRunePanel>());
            }

            runePanel.gameObject.SetActive(true);
            runePanel.Setting(rune);
            runePanel.SetDeck(DeckType.FirstDialDeck);
            runePanel.transform.SetParent(_dialDeckContentTransform);
        }
    }

    private DeckRunePanel GetEmptyPanel()
    {
        return _runePanelList.Find(x => x.IsUse == false);
    }

    private void ReturnPanels()
    {
        List<DeckRunePanel> deckRunePanels = _runePanelList.FindAll(x => x.IsUse == true);
        foreach (DeckRunePanel item in deckRunePanels)
        {
            item.gameObject.SetActive(false);
        }
    }

    public void SetSelectRune(DeckRunePanel rune)
    {
        _selectRune = rune;
        _followObject.SetImage(_selectRune.Rune.GetRune().RuneImage);
        _followObject.gameObject.SetActive(true);
    }

    public void SetTargetRune(DeckRunePanel rune)
    {
        _targetRune = rune;
    }

    public void Equip(DeckType type)
    {
        switch (type)
        {
            case DeckType.FirstDialDeck:
                if (DeckManager.Instance.FirstDialDeck.Count < DeckManager.FIRST_DIAL_DECK_MAX_COUNT)
                {
                    DeckManager.Instance.SetFirstDeck(SelectRune.Rune);
                    DeckManager.Instance.RemoveRune(SelectRune.Rune);
                }
                break;
            case DeckType.OwnDeck:
                DeckManager.Instance.AddRune(SelectRune.Rune);
                DeckManager.Instance.RemoveFirstDeck(SelectRune.Rune);
                break;
            case DeckType.Unknown:
            default:
                break;
        }

        SetSelectRune(null);
        SetTargetRune(null);
    }

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