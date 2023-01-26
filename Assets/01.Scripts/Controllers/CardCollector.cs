using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CardCollector : MonoBehaviour
{
    [SerializeField]
    private MagicCircle _magicCircle;

    [SerializeField]
    private CardsViewUI _deckViewUI = null;

    [SerializeField]
    private CardsViewUI _restViewUI = null;

    [SerializeField]
    private TMP_Text _restAmountText = null;

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
    private int _uiIndex;

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
                _magicCircle.IsBig = true;
            }
            else
            {
                // ÎßåÏïΩ ?†ÌÉù Ïπ¥ÎìúÍ∞Ä ÎßàÎ≤ïÏß??àÏóê ?àÎã§Î©?
                if (Vector2.Distance(SelectCard.GetComponent<RectTransform>().anchoredPosition, _magicCircle.GetComponent<RectTransform>().anchoredPosition)
                <= _magicCircle.CardAreaDistance)
                {
                    Card isAdd = _magicCircle.AddCard(SelectCard);
                    if (isAdd != null)
                    {
                        _handCards.Remove(isAdd);
                        //SelectCard.IsRest = true;
                        _restCards.Add(isAdd);
                        SelectCard.gameObject.SetActive(false);
                    }
                }
                // YES : ÎßàÎ≤ïÏß??àÏóê ?£Í∏∞, Î¶¨Ïä§???àÏóê Ïπ¥Îìú ÏßÄ?∞Í∏∞
                _selectCard.GetComponent<RectTransform>().anchoredPosition = _cardOriginPos;
                _selectCard = value;
                CardSort();
            }
        }
    }

    public IReadOnlyList<Card> DeckCards => _deckCards;
    public IReadOnlyList<Card> RestCards => _restCards;

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
        UIUpdate();
    }

    private void Update()
    {
        if (SelectCard != null)
        {
            SelectCard.GetComponent<RectTransform>().anchoredPosition = Input.mousePosition;

            if (_magicCircle.IsBig == true)
            {
                if (Vector2.Distance(SelectCard.GetComponent<RectTransform>().anchoredPosition, _magicCircle.GetComponent<RectTransform>().anchoredPosition)
                    <= _magicCircle.CardAreaDistance)
                {
                    SelectCard.GetComponent<Image>().sprite = SelectCard.Rune.RuneImage;
                    SelectCard.GetComponent<RectTransform>().sizeDelta = new Vector2(128, 128);
                }
                else
                {
                    // ∏∏æ‡ ≥¢øÏ¥¬ ∞˜¿Ã ∏ﬁ¿Œ¿Ã∏È ∏ﬁ¿Œ ƒ´µÂ, ∫∏¡∂∏È ∫∏¡∂ ƒ´µÂ ¿ÃπÃ¡ˆ ≥÷±‚
                    SelectCard.GetComponent<Image>().sprite = SelectCard.Rune.MainRune.CardImage;
                    SelectCard.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 500);
                }
            }
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

        for (int i = 0; i < _handCards.Count; i++)
        {
            //?¥Í±∏ ?¥Ï§ò??Animation???ÑÌï¥ MagicCircle???êÏãù?ºÎ°ú ?£Ïóà??Í≤ÉÎèÑ ?§Ïãú ???®Ïùò ?êÏãù?ºÎ°ú ?åÏïÑ?Ä ?ïÏÉÅ?ÅÏúºÎ°?SortÍ∞Ä ?òÎäî??Í∑∏Îü¨Î©?DamageÎ∂ÄÎ∂ÑÏóê???§Î•òÍ∞Ä ??Î™?Î£?
            //_handCards[i].transform.SetParent(this.transform); 
            RectTransform rect = _handCards[i].GetComponent<RectTransform>();
            float xDelta = 1440f / _handCards.Count;
            rect.anchoredPosition = new Vector3(i * xDelta + rect.sizeDelta.x / 2, rect.sizeDelta.y / 2, 0);
        }
    }

    public void CardSelect(Card card)
    {
        if (card == null)
        {
            SelectCard.transform.SetSiblingIndex(_uiIndex);
            _uiIndex = -1;
            SelectCard.GetComponent<Image>().sprite = SelectCard.Rune.MainRune.CardImage;
            SelectCard.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 500); // ¿Ã∑± ∫∏∫–µµ ºˆ¡§«ÿæﬂ«‘
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
        Sequence seq = DOTween.Sequence();
        foreach(var card in _handCards)
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
    }

    private void OnDestroy()
    {
        EventManager.StopListening(Define.ON_END_MONSTER_TURN, CoolTimeDecrease);
    }

    private void UIUpdate()
    {
        _deckViewUI.UITextUpdate();
        _restViewUI.UITextUpdate();
    }
}