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

        DeleteRune();

        CreateRune();
    }

    private void CreateRune()
    {
        for(int i = 0; i < _deckSO.RuneList.Count; i++)
        {
            ExplainPanel panel = Managers.Resource.Instantiate("UI/Explain_Panel", _content).GetComponent<ExplainPanel>();
            panel.SetUI(_deckSO.RuneList[i], false);
        }
    }

    private void DeleteRune()
    {
        for(int i = _content.childCount - 1; i >= 0 ; i--)
        {
            Managers.Resource.Destroy(_content.GetChild(i).gameObject);
        }
    }

    public void SelectDeck()
    {
        Managers.Deck.SetDefaultDeck(_deckSO.RuneList);
    }
}
