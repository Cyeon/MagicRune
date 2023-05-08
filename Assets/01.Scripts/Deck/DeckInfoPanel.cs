using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckInfoPanel : MonoBehaviour
{
    [SerializeField]
    private Image _deckImage;
    [SerializeField]
    private TextMeshProUGUI _nameText;
    [SerializeField]
    private TextMeshProUGUI _descText;

    private DeckSO _deckSO;

    private DeckInfoSO _deckInfoSO;

    [SerializeField]
    private Transform _content;

    public void SetInfo(DeckInfoSO info)
    {
        _deckInfoSO = info;

        _deckImage.sprite = info.DeckImage;
        _nameText.SetText(info.DeckName);
        _descText.SetText(info.DeckDescription);

        _deckSO = info.DeckSO;

        // 
    }

    public void SelectDeck()
    {
        Managers.Deck.SetDefaultDeck(_deckSO.RuneList);
    }
}
