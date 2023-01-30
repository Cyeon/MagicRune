using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using NaughtyAttributes;
using System;

public class CardCollector : MonoBehaviour
{
    [SerializeField]
    private MagicCircle _magicCircle;

    [SerializeField, MinValue(0f), MaxValue(1440f)]
    private float _cardAreaDistance;
    [SerializeField]
    private Vector2 _offset;

    [SerializeField]
    private CardsViewUI _deckViewUI = null;

    [SerializeField]
    private CardsViewUI _restViewUI = null;

    [SerializeField]
    private int _cardCnt;

    [SerializeField]
    private CardListSO _deck = null;

    [SerializeField]
    private List<Card> _deckCards = null;

    [SerializeField]
    private List<Card> _handCards = null;

    [SerializeField]
    public List<Card> _restCards = null;

    private List<Card> _tempCards = new List<Card>();

    private Vector2 _cardOriginPos;
    private int _uiIndex;

    private Card _selectCard;
    public Card SelectCard
    {
        get => _selectCard;
        private set
        {
            if (value != null)
            {
                if (_isCardRotate == true) return;

                _selectCard = value;
                _cardOriginPos = _selectCard.GetComponent<RectTransform>().anchoredPosition;
                _magicCircle.IsBig = true;
            }
            else
            {
                if (Input.touchCount == 0) return;
                Card isAdd = null;
                if (Vector2.Distance(_selectCard.GetComponent<RectTransform>().anchoredPosition, _magicCircle.GetComponent<RectTransform>().anchoredPosition)
                <= _magicCircle.CardAreaDistance)
                {
                    if ((_magicCircle.RuneDict.ContainsKey(RuneType.Main) == false && _isFront == true)
                        || (_magicCircle.RuneDict.ContainsKey(RuneType.Main) == true && _isFront == false))
                    {
                        isAdd = _magicCircle.AddCard(SelectCard);
                        if (isAdd != null)
                        {
                            Debug.Log(isAdd);
                            _tempCards.Add(isAdd);
                            _handCards.Remove(isAdd);
                            SelectCard.gameObject.SetActive(false);
                        }
                    }
                }
                Sequence seq = DOTween.Sequence();
                seq.AppendCallback(() =>
                {
                    if (isAdd == null)
                    {
                        _selectCard.SetRune(false);
                    }
                    _selectCard = value;
                });
                seq.Append(_selectCard.GetComponent<RectTransform>().DOAnchorPos(_cardOriginPos, 0.2f));
                seq.InsertCallback(0.2f, () =>
                {
                    _magicCircle.SortCard();
                    CardSort();
                });
            }
        }
    }

    public IReadOnlyList<Card> DeckCards => _deckCards;
    public IReadOnlyList<Card> RestCards => _restCards;

    private bool _isFront = true;
    private bool _isCardRotate = false;

    private void Awake()
    {
        for (int i = 0; i < _deck.cards.Count; i++)
        {
            GameObject go = Instantiate(_deck.cards[i], this.transform);
            Card card = go.GetComponent<Card>();
            card.SetSortingIndex(go.transform.GetSiblingIndex());
            _deckCards.Add(card);
            go.SetActive(false);
            go.name = $"Card_{i + 1}";
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.anchoredPosition = Vector3.zero;
            go.transform.rotation = Quaternion.identity;
        }

        //_cardList = new List<Card>();
        EventManager.StartListening(Define.ON_END_MONSTER_TURN, CoolTimeDecrease);
        EventManager<int>.StartListening(Define.ON_START_PLAYER_TURN, CardDraw);
        EventManager<bool>.StartListening(Define.ON_START_PLAYER_TURN, CardOnOff);
        EventManager<bool>.StartListening(Define.ON_START_MONSTER_TURN, CardOnOff);
        EventManager.StartListening(Define.ON_START_MONSTER_TURN, HandToDeck);

    }

    // 2960 * 1440
    private void Start()
    {
        _isFront = true;
    }

    private void Update()
    {
        if (SelectCard != null)
        {
            if (Input.touchCount <= 0) return;
            SelectCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y - this.GetComponent<RectTransform>().anchoredPosition.y);

            if (_magicCircle.IsBig == true)
            {
                if (Vector2.Distance(Input.GetTouch(0).position, _magicCircle.GetComponent<RectTransform>().anchoredPosition)
                    <= _magicCircle.CardAreaDistance)
                {
                    SelectCard.RuneAreaParent.gameObject.SetActive(true);
                    SelectCard.CardAreaParent.gameObject.SetActive(false);
                    //SelectCard.GetComponent<RectTransform>().sizeDelta = new Vector2(128, 128);
                }
                else
                {
                    SelectCard.RuneAreaParent.gameObject.SetActive(false);
                    SelectCard.CardAreaParent.gameObject.SetActive(true);
                    //SelectCard.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 500);
                }
            }
        }
    }

    private void CardDraw(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (_deckCards.Count <= 0) { break; }
            int idx = UnityEngine.Random.Range(0, _deckCards.Count);
            Card card = _deckCards[idx];
            _deckCards.Remove(card);
            _handCards.Add(card);
            card.gameObject.SetActive(true);
        }
        CardSort();
        UIUpdate();
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

        float xDelta = 1440f / _handCards.Count;
        //float sideArea = (1440f - _cardAreaDistance) / 2; // ���� Scroll Rect�� ���� �Ⱦ� �� ����
        for (int i = 0; i < _handCards.Count; i++)
        {
            //?�걸 ?�줘??Animation???�해 MagicCircle???�식?�로 ?�었??것도 ?�시 ???�의 ?�식?�로 ?�아?�??�상?�으�?Sort가 ?�는??그러�?Damage부분에???�류가 ??�?�?
            //_handCards[i].transform.SetParent(this.transform); 
            RectTransform rect = _handCards[i].GetComponent<RectTransform>();

            rect.anchoredPosition = new Vector3(i * xDelta + rect.sizeDelta.x / 2 + 150 + _offset.x/* + sideArea*/, rect.sizeDelta.y / 2 + _offset.y, 0);
        }
    }

    public void CardSelect(Card card)
    {
        if (Input.touchCount > 1) return;

        if (card == null)
        {
            // ���⼭ �����ɷ��� ī�尡 Null�� �Ǵµ� �� �������� �и�

            //if(_uiIndex == -1)
            //{
            //    SelectCard.transform.SetSiblingIndex(SelectCard.SortingIndex);
            //}
            //else
            //{
            //    SelectCard.transform.SetSiblingIndex(_uiIndex);
            //}
            SelectCard.transform.SetSiblingIndex(SelectCard.SortingIndex);
            SelectCard.SetRune(false);
            _uiIndex = -1;
        }
        else
        {
            _uiIndex = card.transform.GetSiblingIndex();
            card.transform.SetAsLastSibling();
        }
        SelectCard = card;
    }

    private void CoolTimeDecrease()
    {
        for (int i = _restCards.Count - 1; i >= 0; i--)
        {
            Card card = _restCards[i];
            if (card == null) { continue; }
            card.CoolTime--;
            if (card.CoolTime <= 0)
            {
                _deckCards.Add(card);
                _restCards.Remove(card);
            }
        }
        UIUpdate();
    }

    public void CardRotate()
    {
        if (_isCardRotate == true) return;

        transform.DOComplete();

        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() => _isCardRotate = true);
        foreach (var card in _handCards)
        {
            seq.Join(card.transform.DORotate(new Vector3(0, 360, 0), 0.3f, RotateMode.FastBeyond360));
        }

        foreach (var card in _handCards)
        {
            seq.InsertCallback(0.15f, () =>
            {
                card.transform.rotation = Quaternion.Euler(0, 0, 0);
                card.IsFront = !card.IsFront;
            });
        }
        seq.AppendCallback(() =>
        {
            _isCardRotate = false;
            _isFront = _handCards[0].IsFront;
        });

    }

    public void UIUpdate()
    {
        _deckViewUI?.UITextUpdate();
        _restViewUI?.UITextUpdate();
    }

    private void CardOnOff(bool flag)
    {
        foreach (Transform item in transform)
        {
            //Debug.Log(item.name);
            //item.GetComponent<Image>().DOFade(Convert.ToInt32(flag), 1f);
            item.GetComponent<Image>().DOFade(flag ? 1 : 0, 1f);
        }
    }

    private void HandToDeck()
    {
        for (int i = 0; i < _handCards.Count; i++)
        {
            _handCards[i].gameObject.SetActive(false);
            _deckCards.Add(_handCards[i]);
        }
        for (int i = 0; i < _tempCards.Count; i++)
        {
            if (_restCards.Contains(_tempCards[i])) continue;
            _deckCards.Add(_tempCards[i]);
        }
        _handCards.Clear();
    }
    private void OnDestroy()
    {
        EventManager.StopListening(Define.ON_END_MONSTER_TURN, CoolTimeDecrease);
        EventManager<int>.StopListening(Define.ON_START_PLAYER_TURN, CardDraw);
        EventManager<bool>.StopListening(Define.ON_START_PLAYER_TURN, CardOnOff);
        EventManager<bool>.StopListening(Define.ON_START_MONSTER_TURN, CardOnOff);
        EventManager.StopListening(Define.ON_START_MONSTER_TURN, HandToDeck);
        transform.DOKill();
    }
}