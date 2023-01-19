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
                // 만약 선택 카드가 마법진 안에 있다면?
                Vector2 mousePos = Input.mousePosition;
                RectTransform circleRect = _magicCircle.GetComponent<RectTransform>();
                if (mousePos.x >= circleRect.anchoredPosition.x - circleRect.sizeDelta.x / 2 && mousePos.x <= circleRect.anchoredPosition.x + circleRect.sizeDelta.x / 2
                    && mousePos.y >= circleRect.anchoredPosition.y - circleRect.sizeDelta.y / 2 && mousePos.y <= circleRect.anchoredPosition.y + circleRect.sizeDelta.y / 2)
                {
                    bool isAdd = _magicCircle.AddCard(SelectCard);
                    if (isAdd)
                    {
                        _handCards.Remove(SelectCard);
                        _restCards.Add(SelectCard);
                        SelectCard.gameObject.SetActive(false);
                    }
                }
                // YES : 마법진 안에 넣기, 리스트 안에 카드 지우기
                _selectCard.GetComponent<RectTransform>().anchoredPosition = _cardOriginPos;
                _selectCard = value;
                CardSort();
            }
        }
    }

    private void Awake()
    {
        //_cardList = new List<Card>();
        EventManager.StartListening(Define.ON_END_MONSTER_TURN, CoolTimeDecrease);
    }

    // 2960 * 1440
    private void Start()
    {
        for (int i = 0; i < _deck.cards.Count; i++)
        {
            GameObject go = Instantiate(_deck.cards[i], this.transform);
            _deckCards.Add(go.GetComponent<Card>());
            go.SetActive(false);
        }

        for (int i = 0; i < _cardCnt; i++)
        {
            GameObject go = _deckCards[i].gameObject /*Instantiate(_cardTemplate.gameObject, this.transform)*/;
            _deckCards.Remove(_deckCards[i]);
            go.SetActive(true);
            RectTransform rect = go.GetComponent<RectTransform>();
            _handCards.Add(go.GetComponent<Card>());
            rect.anchoredPosition = Vector3.zero;
            go.transform.rotation = Quaternion.identity;
        }

        CardSort();
    }

    private void Update()
    {
        if (SelectCard != null)
        {
            SelectCard.GetComponent<RectTransform>().anchoredPosition = Input.mousePosition;
        }

        if (Input.GetKeyDown(KeyCode.Space)) // 작동?안함?
        {
            Debug.Log(1);

            for (int i = 0; i < _cardCnt; i++)
            {
                GameObject go = Instantiate(_cardTemplate.gameObject, this.transform);
                RectTransform rect = go.GetComponent<RectTransform>();
                _handCards.Add(go.GetComponent<Card>());
                rect.anchoredPosition = Vector3.zero;
                go.transform.rotation = Quaternion.identity;
            }

            CardSort();
        }
    }

    public void CardSort()
    {
        for (int i = 0; i < _handCards.Count; i++)
        {
            RectTransform rect = _handCards[i].GetComponent<RectTransform>();
            float xDelta = 1440f / _handCards.Count;
            rect.anchoredPosition = new Vector3(i * xDelta + rect.sizeDelta.x / 2, rect.sizeDelta.y / 2, 0);
        }
    }

    public void CardSelect(Card card)
    {
        SelectCard = card;
    }

    private void CoolTimeDecrease()
    {
        foreach (Card card in _restCards)
        {
            card.CoolTime--;
            if (!card.IsRest)
            {
                _deckCards.Add(card);
                _restCards.Remove(card);
            }
        }
    }
    private void OnDestroy()
    {
        EventManager.StopListening(Define.ON_END_MONSTER_TURN, CoolTimeDecrease);
    }
}