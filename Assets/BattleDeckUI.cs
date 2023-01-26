using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleDeckUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _deckAmountText = null;

    [SerializeField]
    private CardCollector _cardCollector = null;

    [SerializeField]
    private Transform _content = null;

    [SerializeField]
    private GameObject _scrollView = null;

    private void SetMyChild()
    {
        for (int i = 0; i < _cardCollector.DeckCards.Count; i++)
        {
            GameObject gameObject = _cardCollector.DeckCards[i].gameObject;
            gameObject.GetComponent<Card>().enabled = false;
            gameObject.SetActive(true);
            gameObject.transform.SetParent(_content);
            gameObject.transform.localScale = Vector3.one;
        }
    }

    private void ReturnChild()
    {
        for (int i = 0; i < _cardCollector.DeckCards.Count; i++)
        {
            GameObject gameObject = _cardCollector.DeckCards[i].gameObject;
            gameObject.GetComponent<Card>().enabled = true;  
            gameObject.SetActive(false);
            gameObject.transform.SetParent(_cardCollector.transform);
        }
    }

    public void UITextUpdate()
    {
        _deckAmountText.text = _cardCollector.DeckCards.Count.ToString();
    }

    public void OpenUI()
    {
        SetMyChild();
        _scrollView.SetActive(true);
    }

    public void CloseUI()
    {
        ReturnChild();
        _scrollView.SetActive(false);
    }
}