using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardsViewUI : MonoBehaviour
{
    [SerializeField]
    private CardCollector _cardCollector = null;

    [SerializeField]
    private bool _isRest = false;

    [SerializeField]            
    private Color _onColor = new Color(0f, 0f, 0f, 1f);

    [Header("GameObjects")]
    [SerializeField]
    private GameObject _scrollView = null;

    [SerializeField]
    private Transform _content = null;

    [SerializeField]
    private GameObject _shadowPanel = null;

    [SerializeField]
    private TMP_Text _amountText = null;

    [SerializeField]
    private GameObject _otherCardsView = null;

    [SerializeField]
    private Button _reverseButton = null;

    private Color _offColor = new Color(0f, 0f, 0f, 0f);

    private Image _shadowPanelImage = null;

    private void Start()
    {
        EventManager.StartListening(Define.CLICK_VIEW_UI, ClickCard);
        _reverseButton.onClick.AddListener(() => CardReverse());
        _shadowPanelImage = _shadowPanel.GetComponent<Image>();
        _shadowPanelImage.color = _offColor;
        _shadowPanelImage.raycastTarget = false;
    }

    private void SetChild(List<Card> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            GameObject gameObject = cards[i].gameObject;

            if (gameObject == null) { continue; }

            gameObject.GetComponent<ViewCard>().isActive = true;

            Card card = gameObject.GetComponent<Card>();
            card.SetRune(false);
            card.enabled = false;

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
            if (gameObject == null) { continue; }

            gameObject.GetComponent<ViewCard>().isActive = false;

            gameObject.GetComponent<Card>().enabled = true;
            gameObject.SetActive(false);
            gameObject.transform.rotation = Quaternion.identity;
            gameObject.transform.SetParent(_cardCollector.transform);
        }
    }

    public void UITextUpdate()
    {
        if (_isRest)
        {
            _amountText.text = _cardCollector.RestCards.Count.ToString();
        }
        else
            _amountText.text = _cardCollector.DeckCards.Count.ToString();
    }

    public void OpenUI()
    {
        if (_isRest)
            SetChild((List<Card>)_cardCollector.RestCards);
        else
            SetChild((List<Card>)_cardCollector.DeckCards);

        _scrollView.SetActive(true);
        _otherCardsView.SetActive(false);
    }

    public void CloseUI()
    {
        if (_isRest)
            ReturnChild((List<Card>)_cardCollector.RestCards);
        else
            ReturnChild((List<Card>)_cardCollector.DeckCards);

        _scrollView.SetActive(false);
        _otherCardsView.SetActive(true);
    }

    private void ClickCard()
    {
        if (_scrollView.activeSelf)
        {
            _shadowPanelImage.color = _onColor;
            _shadowPanelImage.raycastTarget = true;
        }
    }

    public void ClickPanel()
    {
        if (_scrollView.activeSelf)
        {
            _shadowPanelImage.color = _offColor;
            _shadowPanelImage.raycastTarget = false;
            _content.parent.Find("Card_Temp").GetComponent<ViewCard>().DestroySelf();
        }
    }

    public void CardReverse()
    {
        foreach (Transform item in _content)
        {
            Card card = item.GetComponent<Card>();
            card.enabled = true;
            card.IsFront = !card.IsFront;
            card.enabled = false;
        }
    }

    private void OnDestroy()
    {
        EventManager.StopListening(Define.CLICK_VIEW_UI, ClickCard);
    }
}