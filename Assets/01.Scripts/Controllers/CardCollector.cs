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
                        //SelectCard.IsRest = true;
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
            go.name = $"Card_{i + 1}";
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.anchoredPosition = Vector3.zero;
            go.transform.rotation = Quaternion.identity;
        }

        CardDraw(_cardCnt);

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
            CardDraw(_deckCards.Count);
        }
    }

    private void CardDraw(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int idx = Random.Range(0, _deckCards.Count);
            Card card = _deckCards[idx];
            _deckCards.Remove(card);
            _handCards.Add(card);
            card.gameObject.SetActive(true);
        }
        CardSort();
    }

    private void CardSort()
    {
        _handCards.Sort(delegate (Card a, Card b)
        {
            int aName = int.Parse(a.gameObject.name.ToString().Substring(5, a.gameObject.name.Length - 5));
            int bName = int.Parse(b.gameObject.name.ToString().Substring(5, b.gameObject.name.Length - 5));
            if (aName > bName) { return 1; }
            return -1;
        });

        for (int i = 0; i < _handCards.Count; i++)
        {
            //이걸 해줘야 Animation을 위해 MagicCircle의 자식으로 넣었던 것도 다시 손 패의 자식으로 돌아와 정상적으로 Sort가 되는데 그러면 Damage부분에서 오류가 남 몰?루
            //_handCards[i].transform.SetParent(this.transform); 
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
        for (int i = _restCards.Count - 1; i >= 0; i--)
        {
            Card card = _restCards[i];
            card.CoolTime--;
            if (card.CoolTime <= 0)
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