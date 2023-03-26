using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum DeckType
{
    FirstDial,
    Inventory,
    Unknown
}
public class DeckSettingUI : MonoBehaviour
{
    private List<DeckRunePanel> _runePanelList = new List<DeckRunePanel>(35);

    private Dictionary<DeckType, DeckRunePanel> _runeDeckTypeDict = new Dictionary<DeckType, DeckRunePanel>();

    [SerializeField]
    private GameObject _runePanelPrefab = null;
    [SerializeField]
    private Transform _ownDeckTransform = null;
    [SerializeField]
    private Transform _dialDeckTransform = null;

    private DeckRunePanel _activeRunePanel = null;
    public DeckRunePanel ActiveRune => _activeRunePanel;

    private void Awake()
    {
        if (_runePanelPrefab != null)
        {
            for (int i = 0; i < 30; i++)
            {
                GameObject game = Instantiate(_runePanelPrefab, transform);
                _runePanelList.Add(game.GetComponent<DeckRunePanel>());
            }
        }
        SettingAllDeck();
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
            if (runePanel == null)
            {
                GameObject game = Instantiate(_runePanelPrefab, transform);
                _runePanelList.Add(game.GetComponent<DeckRunePanel>());
            }

            runePanel.Setting(rune);
            runePanel.SetParent(_ownDeckTransform);
        }
    }

    private void SetDialDeck()
    {
        foreach (Rune rune in DeckManager.Instance.FirstDialDeck)
        {
            DeckRunePanel runePanel = GetEmptyPanel();
            if (runePanel == null)
            {
                GameObject game = Instantiate(_runePanelPrefab, transform);
                _runePanelList.Add(game.GetComponent<DeckRunePanel>());
            }

            runePanel.Setting(rune);
            runePanel.SetParent(_dialDeckTransform);
        }
    }

    private DeckRunePanel GetEmptyPanel()
    {
        return _runePanelList.Find(x => x.IsUse == false);
    }

    public void SwitchRune()
    {

    }

    public void SetActiveRune(DeckRunePanel runePanel)
    {
        _activeRunePanel = runePanel;
    }
}