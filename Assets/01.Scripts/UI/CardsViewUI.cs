using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardsViewUI : MonoBehaviour
{
    [SerializeField]
    private CardCollector _cardCollector = null;

    [SerializeField]
    private GameObject _scrollView = null;

    [SerializeField]
    private Transform _content = null;

    [SerializeField]
    private TMP_Text _amountText = null;

    [SerializeField]
    private bool _isRest = false;

    private void SetChild(List<Card> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            GameObject gameObject = cards[i].gameObject;
            gameObject.GetComponent<Card>().enabled = false;
            gameObject.SetActive(true);
            gameObject.transform.SetParent(_content);
            gameObject.transform.localScale = Vector3.one;
        }
    }

    private void ReturnChild(List<Card> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            GameObject gameObject = cards[i].gameObject;
            gameObject.GetComponent<Card>().enabled = true;
            gameObject.SetActive(false);
            gameObject.transform.SetParent(_cardCollector.transform);
        }
    }

    public void UITextUpdate()
    {
        _amountText.text = _cardCollector.DeckCards.Count.ToString();
    }

    public void OpenUI()
    {
        if (_isRest)
            SetChild((List<Card>)_cardCollector.RestCards);
        else
            SetChild((List<Card>)_cardCollector.DeckCards);
        
        _scrollView.SetActive(true);
    }

    public void CloseUI()
    {
        if (_isRest)
            ReturnChild((List<Card>)_cardCollector.RestCards);
        else
            ReturnChild((List<Card>)_cardCollector.DeckCards);

        _scrollView.SetActive(false);
    }
}