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

    private int _selectArea = -1;
    private int _selectIndex = -1;

    [SerializeField]
    private Image _selectButton;
    [SerializeField]
    private TextMeshProUGUI _selectText;

    [SerializeField]
    private Image _deckDisplayImage;

    public void SetInfo(DeckInfoSO info, int index)
    {
        _deckInfoSO = info;
        _selectArea = index;

        _deckImage.sprite = info.DeckImage;
        _nameText.SetText(info.DeckName);
        _descText.SetText(info.DeckDescription);

        _deckSO = info.DeckSO;

        UpdateSelectText(_selectArea == _selectIndex);

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
        _selectIndex = _selectArea;

        UpdateSelectText(_selectArea == _selectIndex);
        _deckDisplayImage.sprite = _deckInfoSO.DeckImage;
    }

    private void UpdateSelectText(bool value)
    {
        if (value == true)
        {
            _selectButton.color = Color.gray;
            _selectText.SetText("º±≈√µ ");
        }
        else
        {
            _selectButton.color = Color.white;
            _selectText.SetText("º±≈√");
        }
    }
}
