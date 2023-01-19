using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCollector : MonoBehaviour
{
    [SerializeField]
    private MagicCircle _magicCircle;

    [SerializeField]
    private Card _cardTemplate;
    [SerializeField]
    private int _cardCnt;

    //private List<Card> _cardList;
    [SerializeField]
    private CardListSO _deck = null;

    [SerializeField]
    private List<Card> _deckCards = null;

    [SerializeField]
    private List<Card> _handCards = null;

    [SerializeField]
    private List<Card> _restCards = null;

    private Vector2 _cardOriginPos;
    private Card _selectCard;
    public Card SelectCard
    {
        get => _selectCard;
        private set
        {
            if (value != null)
            {
                _selectCard = value;
                _cardOriginPos = _selectCard.GetComponent<RectTransform>().anchoredPosition;
            }
            else
            {
                // ���� ���� ī�尡 ������ �ȿ� �ִٸ�?
                Vector2 mousePos = Input.mousePosition;
                RectTransform circleRect = _magicCircle.GetComponent<RectTransform>();
                if (mousePos.x >= circleRect.anchoredPosition.x - circleRect.sizeDelta.x / 2 && mousePos.x <= circleRect.anchoredPosition.x + circleRect.sizeDelta.x / 2
                    && mousePos.y >= circleRect.anchoredPosition.y - circleRect.sizeDelta.y / 2 && mousePos.y <= circleRect.anchoredPosition.y + circleRect.sizeDelta.y / 2)
                {
                    bool isAdd = _magicCircle.AddCard(SelectCard);
                    if (isAdd)
                    {
                        //_handCards.cards.Remove(SelectCard);
                        //_cardList.Remove(SelectCard);
                        Destroy(SelectCard.gameObject);
                    }
                }
                // YES : ������ �ȿ� �ֱ�, ����Ʈ �ȿ� ī�� �����
                _selectCard.GetComponent<RectTransform>().anchoredPosition = _cardOriginPos;
                _selectCard = value;
                CardSort();
            }
        }
    }

    private void Awake()
    {
        CardListClear();
        for (int i = 0; i < _deck.cards.Count; i++)
        {
            GameObject card = Instantiate(_deck.cards[i], this.transform);
            _deckCards.Add(card.GetComponent<Card>());
            card.SetActive(false);
        }

        //_cardList = new List<Card>();
        EventManager.StartListening(Define.ON_START_PLAYER_TURN, CardCreate);
        EventManager.StartListening(Define.ON_END_PLAYER_TURN, CardDestroy);
        EventManager.StartListening(Define.ON_END_MONSTER_TURN, CoolTimeDecrease);
    }

    private void Update()
    {
        if (SelectCard != null)
        {
            SelectCard.GetComponent<RectTransform>().anchoredPosition = Input.mousePosition;
        }

        if (Input.GetKeyDown(KeyCode.Space)) // �۵�?����?
        {
            //Debug.Log(1);

            CardCreate();
        }

    }

    private void CardListClear()
    {
        _deckCards.Clear();
        _handCards.Clear();
        _restCards.Clear();
    }

    public void CardSort()
    {
        for (int i = 0; i < _handCards.Count; i++)
        {
            RectTransform rect = _handCards[i].gameObject.GetComponent<RectTransform>();
            float xDelta = 1440f / _handCards.Count;
            rect.anchoredPosition = new Vector3(i * xDelta + rect.sizeDelta.x / 2, rect.sizeDelta.y / 2, 0);
        }
    }

    public void CardSelect(Card card)
    {
        SelectCard = card;
    }

    public void CardDraw()
    {
        if (_deckCards.Count == 0)
        {
            Debug.LogWarning("Deck is Empty");
            return;
        }

        int idx = Random.Range(0, _deckCards.Count);
        Debug.Log(idx);
        Card card = _deckCards[idx];
        _deckCards.Remove(card);
        _handCards.Add(card);
    }

    public void CardCreate()
    {
        for (int i = 0; i < _cardCnt; i++)
        {
            CardDraw();

            GameObject go = _handCards[i].gameObject/*Instantiate(_handCards[_handCards.Count - 1].CardPrefab, this.transform)*/;
            go.SetActive(true);
            RectTransform rect = go.GetComponent<RectTransform>();
            //_cardList.Add(go.GetComponent<Card>());
            rect.anchoredPosition = Vector3.zero;
            go.transform.rotation = Quaternion.identity;
        }
        CardSort();
    }

    public void CardDestroy()
    {
        for (int i = 0; i < _handCards.Count; i++)
        {
            Card card = _handCards[i];

            _deckCards.Add(card);
            _handCards[i].gameObject.SetActive(false);
            //Destroy(_handCards[i].gameObject);
            //Debug.Log(card.gameObject.name);
            //Destroy(card.gameObject);
        }
        _handCards.Clear();
    }

    private void CoolTimeDecrease()
    {
        foreach (Card card in _restCards)
        {
            card.CoolTime--;
            if (!card.IsRest)
                RestCardToDeck(card);
        }
    }

    private void RestCardToDeck(Card card)
    {
        _deckCards.Add(card);
        _restCards.Remove(card);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(Define.ON_START_PLAYER_TURN, CardCreate);
        EventManager.StopListening(Define.ON_END_PLAYER_TURN, CardDestroy);
        EventManager.StopListening(Define.ON_END_MONSTER_TURN, CoolTimeDecrease);
    }
}