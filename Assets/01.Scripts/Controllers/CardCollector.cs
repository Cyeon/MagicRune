using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using NaughtyAttributes;
using System;
using MyBox;
using MinValue = NaughtyAttributes.MinValueAttribute;
using MaxValue = NaughtyAttributes.MaxValueAttribute;
using System.Linq;
//using static UnityEditor.PlayerSettings;

public class CardCollector : MonoBehaviour
{
    [SerializeField]
    private MagicCircle _magicCircle;
    public MagicCircle MagicCircle => _magicCircle;

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
    private int _fingerID = -1;
    public int FingerID
    {
        get => _fingerID;
        set
        {
            _fingerID = value;
            if (_fingerID == -1)
            {
                if (_selectCard != null)
                {
                    Card isAdd = null;
                    if (Vector2.Distance(_selectCard.GetComponent<RectTransform>().anchoredPosition, _magicCircle.GetComponent<RectTransform>().anchoredPosition)
                    <= _magicCircle.CardAreaDistance)
                    {
                        bool main = _magicCircle.RuneDict.ContainsKey(RuneType.Main) == false && _isFront == true;
                        bool assist = _magicCircle.RuneDict.ContainsKey(RuneType.Main) == true && _isFront == false;
                        if (main || assist)
                        {
                            isAdd = _magicCircle.AddCard(SelectCard);
                            if (isAdd != null)
                            {
                                //Debug.Log(isAdd);
                                _tempCards.Add(isAdd);
                                _handCards.Remove(isAdd);
                                UpdateCardOutline();
                                //SelectCard.gameObject.SetActive(false);
                            }
                        }
                        else
                        {
                            if (_magicCircle.RuneDict.ContainsKey(RuneType.Main) == false && _isFront == false)
                            {
                                UIManager.Instance.InfoMessagePopup("¸ÞÀÎ ·éÀ» ³Ö¾îÁÖ¼¼¿ä.", GetTouchPos());
                            }
                            else if (_magicCircle.RuneDict.ContainsKey(RuneType.Assist) == true && _isFront == true)
                            {
                                UIManager.Instance.InfoMessagePopup("º¸Á¶ ·éÀ» ³Ö¾îÁÖ¼¼¿ä.", GetTouchPos());
                            }
                        }
                    }
                    Sequence seq = DOTween.Sequence();
                    seq.AppendCallback(() =>
                    {
                        if (isAdd == null && _selectCard != null)
                        {
                            _selectCard.SetRune(false);
                        }
                    });
                    if (_selectCard != null)
                    {
                        if (_cardOriginPos == null)
                        {
                            _cardOriginPos = Vector2.zero;
                        }

                        seq.Append(_selectCard.GetComponent<RectTransform>().DOAnchorPos(_cardOriginPos, 0.2f));
                        seq.InsertCallback(0.2f, () =>
                        {
                            _magicCircle.SortCard();
                            CardSort();
                        });
                    }
                    else
                    {
                        seq.AppendCallback(() =>
                        {
                            _magicCircle.SortCard();
                            CardSort();
                        });
                    }
                }
            }
        }
    }

    public Card _selectCard;
    public Card SelectCard
    {
        get => _selectCard;
        private set
        {
            if (value != null)
            {
                if (_isCardRotate == true) return;

                _selectCard = value;
                //_selectCard.transform.localScale = new Vector3(2f, 2f, 1f);
                _cardOriginPos = _selectCard.GetComponent<RectTransform>().anchoredPosition;
                _magicCircle.IsBig = true;
            }
            else
            {
                if (_selectCard != null)
                {
                    _selectCard = value;
                    //_selectCard.transform.localScale = Vector3.one;
                    //if (Input.touchCount == 0) return;
                    //Card isAdd = null;
                    //if (Vector2.Distance(_selectCard.GetComponent<RectTransform>().anchoredPosition, _magicCircle.GetComponent<RectTransform>().anchoredPosition)
                    //<= _magicCircle.CardAreaDistance)
                    //{
                    //    bool main = _magicCircle.RuneDict.ContainsKey(RuneType.Main) == false && _isFront == true;
                    //    bool assist = _magicCircle.RuneDict.ContainsKey(RuneType.Main) == true && _isFront == false;
                    //    if (main || assist)
                    //    {
                    //        isAdd = _magicCircle.AddCard(SelectCard);
                    //        if (isAdd != null)
                    //        {
                    //            //Debug.Log(isAdd);
                    //            _tempCards.Add(isAdd);
                    //            _handCards.Remove(isAdd);
                    //            UpdateCardOutline();
                    //            //SelectCard.gameObject.SetActive(false);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (_magicCircle.RuneDict.ContainsKey(RuneType.Main) == false && _isFront == false)
                    //        {
                    //            UIManager.Instance.InfoMessagePopup("¸ÞÀÎ ·éÀ» ³Ö¾îÁÖ¼¼¿ä.", GetTouchPos());
                    //        }
                    //        else if (_magicCircle.RuneDict.ContainsKey(RuneType.Assist) == true && _isFront == true)
                    //        {
                    //            UIManager.Instance.InfoMessagePopup("º¸Á¶ ·éÀ» ³Ö¾îÁÖ¼¼¿ä.", GetTouchPos());
                    //        }
                    //    }
                    //}
                    //Sequence seq = DOTween.Sequence();
                    //seq.AppendCallback(() =>
                    //{
                    //    if (isAdd == null)
                    //    {
                    //        _selectCard.SetRune(false);
                    //    }
                    //    _selectCard = value;
                    //});
                    //if (_selectCard != null)
                    //{
                    //    if (_cardOriginPos == null)
                    //    {
                    //        _cardOriginPos = Vector2.zero;
                    //    }

                    //    seq.Append(_selectCard.GetComponent<RectTransform>().DOAnchorPos(_cardOriginPos, 0.2f));
                    //    seq.InsertCallback(0.2f, () =>
                    //    {
                    //        _magicCircle.SortCard();
                    //        CardSort();
                    //    });
                    //}
                    //else
                    //{
                    //    seq.AppendCallback(() =>
                    //    {
                    //        _magicCircle.SortCard();
                    //        CardSort();
                    //    });
                    //}

                }
            }
        }
    }

    public IReadOnlyList<Card> DeckCards => _deckCards;
    public IReadOnlyList<Card> RestCards => _restCards;

    private bool _isFront = true;
    public bool IsFront { get => _isFront; set => _isFront = value; }
    private bool _isCardRotate = false;

    [SerializeField]
    private GameObject _cardTemplate = null;

    [SerializeField]
    private RectTransform _myCardLeft;
    [SerializeField]
    private RectTransform _myCardRight;
    [SerializeField]
    private RectTransform _myCardCenter;

    private void Awake()
    {
        for (int i = 0; i < _deck.cards.Count; i++)
        {
            GameObject go = Instantiate(_cardTemplate, this.transform);
            Card card = go.GetComponent<Card>();
            card.SetRune(_deck.cards[i]);
            card.SetSortingIndex(go.transform.GetSiblingIndex());
            _deckCards.Add(card);
            go.SetActive(false);
            go.name = $"Card_{i + 1}";
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.anchoredPosition = Vector3.zero;
            go.transform.rotation = Quaternion.identity;
        }

        //EventManager.StartListening(Define.ON_END_MONSTER_TURN, CoolTimeDecrease);
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
            if (_fingerID == -1) return;
            Touch t = Input.GetTouch(FingerID);
            //SelectCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y - this.GetComponent<RectTransform>().anchoredPosition.y);
            //float widthPercent = t.position.x * 100 / 1440f;
            //float heightPercent = t.position.y * 100 / 2960f;
            //SelectCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(t.position.x - widthPercent * 300, t.position.y - widthPercent * 500);
            SelectCard.GetComponent<RectTransform>().anchoredPosition = t.position;

            if (_magicCircle.IsBig == true)
            {
                if (Vector2.Distance(t.position, _magicCircle.GetComponent<RectTransform>().anchoredPosition)
                    <= _magicCircle.CardAreaDistance)
                {
                    SelectCard.RuneAreaParent.gameObject.SetActive(true);
                    SelectCard.CardAreaParent.gameObject.SetActive(false);
                    SelectCard.AssistRune.gameObject.SetActive(false);
                    //SelectCard.GetComponent<RectTransform>().sizeDelta = new Vector2(128, 128);
                }
                else
                {
                    SelectCard.RuneAreaParent.gameObject.SetActive(false);
                    SelectCard.CardAreaParent.gameObject.SetActive(true);
                    SelectCard.AssistRune.gameObject.SetActive(true);
                    //SelectCard.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 500);
                }
            }
        }
    }

    public void CardDraw(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (_deckCards.Count <= 0)
            {
                _deckCards = _restCards.ToList();
                _restCards.Clear();

                foreach (var c in _deckCards)
                {
                    c.IsRest = false;
                }
            }
            int idx = UnityEngine.Random.Range(0, _deckCards.Count);
            Card card = _deckCards[idx];
            if (!_handCards.Contains(card))
            {
                _handCards.Add(card);
            }
            _deckCards.Remove(card);
            card.gameObject.SetActive(true);
            card.IsFront = true;
        }
        CardSort();
        UIUpdate();
    }

    public void CardSort()
    {
        _handCards.Sort(delegate (Card a, Card b)
        {
            int aName = int.Parse(a.gameObject.name.ToString().Substring(5, a.gameObject.name.Length - 5));
            int bName = int.Parse(b.gameObject.name.ToString().Substring(5, b.gameObject.name.Length - 5));
            if (aName > bName) { return 1; }
            return -1;
        });

        // ±âÁ¸ ÄÚµå
        //float xDelta = 1440f / _handCards.Count;
        ////float sideArea = (1440f - _cardAreaDistance) / 2; // ï¿½ï¿½ï¿½ï¿½ Scroll Rectï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½È¾ï¿½ ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
        //for (int i = 0; i < _handCards.Count; i++)
        //{
        //    //?ï¿½ê±¸ ?ï¿½ì¤˜??Animation???ï¿½í•´ MagicCircle???ï¿½ì‹?ï¿½ë¡œ ?ï¿½ì—ˆ??ê²ƒë„ ?ï¿½ì‹œ ???ï¿½ì˜ ?ï¿½ì‹?ï¿½ë¡œ ?ï¿½ì•„?ï¿??ï¿½ìƒ?ï¿½ìœ¼ï¿?Sortê°€ ?ï¿½ëŠ”??ê·¸ëŸ¬ï¿?Damageë¶€ë¶„ì—???ï¿½ë¥˜ê°€ ??ï¿?ï¿?
        //    //_handCards[i].transform.SetParent(this.transform); 
        //    RectTransform rect = _handCards[i].GetComponent<RectTransform>();
        //    rect.anchoredPosition = new Vector3(i * xDelta + rect.sizeDelta.x / 2 + 150 + _offset.x, rect.sizeDelta.y / 2 + _offset.y, 0);
        //    rect.rotation = Quaternion.Euler(Vector3.zero);
        //    rect.localScale = Vector3.one;
        //    _handCards[i].IsFront = _isFront;
        //}

        // °í¶ó´ÏÇ¥ ÄÚµå
        List<PRS> originCardPRS = new List<PRS>();
        originCardPRS = RoundSort(_myCardLeft, _myCardRight, _handCards.Count, 0.5f, Vector3.one);

        for (int i = 0; i < _handCards.Count; i++)
        {
            Card targetCard = _handCards[i];
            targetCard.OriginPRS = originCardPRS[i];
            targetCard.MoveTransform(targetCard.OriginPRS, true, 0.7f);
        }
        HandCardOutline(false);
        UpdateCardOutline();
    }

    public List<PRS> RoundSort(RectTransform leftRect, RectTransform rightRect, int cardCount, float height, Vector3 scale)
    {
        float[] cardLerp = new float[cardCount];
        List<PRS> result = new List<PRS>(cardCount);

        switch (cardCount)
        {
            // 1 ~ 3 Àº ÇÏµåÄÚµùÇÑ°Í Á÷Á¢º¸¸é¼­ ¼öÄ¡°ª³ÖÀº°Í µÇ¸é ³ªÁß¿¡´Â ¼öÁ¤ÇÒ µí
            case 1:
                cardLerp = new float[] { 0.5f };
                break;
            case 2:
                cardLerp = new float[] { 0.27f, 0.73f };
                break;
            case 3:
                cardLerp = new float[] { 0.1f, 0.5f, 0.9f };
                break;
            default:
                float interval = 1f / (cardCount - 1);
                for(int i = 0; i < cardCount; i++)
                {
                    cardLerp[i] = interval * i;
                }
                break;
        }

        for(int i = 0; i < cardCount; i++)
        {
            Vector3 targetPos;
            if (cardCount >= 4)
            {
                targetPos = Vector3.Slerp(leftRect.anchoredPosition3D, rightRect.anchoredPosition3D, cardLerp[i]);
            }
            else
            {
                targetPos = Vector3.Lerp(leftRect.anchoredPosition3D, rightRect.anchoredPosition3D, cardLerp[i]);
            }
            Quaternion targetRot = Quaternion.identity;
            if(cardCount >= 4)
            {
                float curve = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(cardLerp[i] - 0.5f, 2));
                //curve = height >= 0 ? curve : -curve; // ÀÌ°Ç µÚÁýÀ»¶§ Áö±ÝÀº ÇÊ¿ä¾øÀ¸
                targetPos.y += curve;
                targetRot = Quaternion.Slerp(leftRect.rotation, rightRect.rotation, cardLerp[i]);
            }
            result.Add(new PRS(targetPos, targetRot, scale));
        }
        return result;
    }

    public void UpdateCardOutline()
    {
        for (int i = 0; i < _handCards.Count; i++)
        {
            if (IsFront == true)
            {
                if (MagicCircle.RuneDict.ContainsKey(RuneType.Main) == true)
                {
                    _handCards[i].SetOutline(false);
                }
                else
                {
                    if (DummyCost.Instance.CanRune(_handCards[i].Rune.MainRune.Cost))
                    {
                        //_handCards[i].SetOutlineColor(Color.cyan);
                        _handCards[i].SetOutline(true);
                    }
                    else
                    {
                        //_handCards[i].SetOutlineColor(Color.clear);
                        _handCards[i].SetOutline(false);
                    }
                }
            }
            else
            {
                if (MagicCircle.RuneDict.ContainsKey(RuneType.Main) == true)
                {
                    if (MagicCircle.RuneDict.ContainsKey(RuneType.Assist))
                    {
                        bool full = true;
                        foreach (var list in MagicCircle.RuneDict[RuneType.Assist])
                        {
                            if (list.Rune == null)
                            {
                                full = false;
                                break;
                            }
                        }

                        if (full == true)
                        {
                            _handCards[i].SetOutline(false);
                        }
                        else
                        {
                            if (DummyCost.Instance.CanRune(_handCards[i].Rune.AssistRune.Cost))
                            {
                                //_handCards[i].SetOutlineColor(Color.cyan);
                                _handCards[i].SetOutline(true);
                            }
                            else
                            {
                                //_handCards[i].SetOutlineColor(Color.clear);
                                _handCards[i].SetOutline(false);
                            }
                        }
                    }
                    else
                    {
                        if (DummyCost.Instance.CanRune(_handCards[i].Rune.AssistRune.Cost))
                        {
                            //_handCards[i].SetOutlineColor(Color.cyan);
                            _handCards[i].SetOutline(true);
                        }
                        else
                        {
                            //_handCards[i].SetOutlineColor(Color.clear);
                            _handCards[i].SetOutline(false);
                        }
                    }
                }
                else
                {
                    _handCards[i].SetOutline(false);
                }
            }
        }
    }

    public void HandCardOutline(bool value)
    {
        foreach (var c in _handCards)
        {
            c.SetOutline(value);
        }
    }

    public void HandCardSetRune(bool value)
    {
        foreach (var c in _handCards)
        {
            c.SetRune(value);
        }
    }

    public void CardSelect(Card card)
    {
        if (Input.touchCount > 1) return;
        if (_isCardRotate == true) return;

        if (card == null)
        {
            // ï¿½ï¿½ï¿½â¼­ ï¿½ï¿½ï¿½ï¿½ï¿½É·ï¿½ï¿½ï¿½ Ä«ï¿½å°¡ Nullï¿½ï¿½ ï¿½Ç´Âµï¿½ ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½Ð¸ï¿½

            //if(_uiIndex == -1)
            //{
            //    SelectCard.transform.SetSiblingIndex(SelectCard.SortingIndex);
            //}
            //else
            //{
            //    SelectCard.transform.SetSiblingIndex(_uiIndex);
            //}
            if (SelectCard != null)
            {
                SelectCard.transform.SetSiblingIndex(SelectCard.SortingIndex);
                SelectCard.SetRune(false);
            }
            _uiIndex = -1;
        }
        else
        {
            _uiIndex = card.SortingIndex;
            card.transform.SetAsLastSibling();
        }
        SelectCard = card;
    }

    //private void CoolTimeDecrease()
    //{
    //    for (int i = _restCards.Count - 1; i >= 0; i--)
    //    {
    //        Card card = _restCards[i];
    //        if (card == null) { continue; }
    //        card.CoolTime--;
    //        if (card.CoolTime <= 0)
    //        {
    //            _deckCards.Add(card);
    //            _restCards.Remove(card);
    //        }
    //    }
    //    UIUpdate();
    //}

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
            if (_handCards.Count > 0)
            {
                _isFront = _handCards[0].IsFront;
            }
            else
            {
                _isFront = true;
            }
            UpdateCardOutline();
            CardSort();
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
            Card targetCard;
            TryGetComponent<Card>(out targetCard);
            if(targetCard != null)
            {
                CanvasGroup cvGroup = item.Find("Card_Area").GetComponent<CanvasGroup>();
                targetCard.SetRune(false);
                cvGroup.alpha = !flag ? 1 : 0;
                cvGroup.DOFade(flag ? 1 : 0, 1f);
            }
            
        }
    }

    private void HandToDeck()
    {
        for (int i = 0; i < _handCards.Count; i++)
        {
            _handCards[i].gameObject.SetActive(false);
            if (!_deckCards.Contains(_handCards[i]))
                _deckCards.Add(_handCards[i]);
        }
        for (int i = 0; i < _tempCards.Count; i++)
        {
            if (_restCards.Contains(_tempCards[i])) continue;
            if (!_deckCards.Contains(_tempCards[i]))
                _deckCards.Add(_tempCards[i]);
        }
        _handCards.Clear();
    }
    private void OnDestroy()
    {
        //EventManager.StopListening(Define.ON_END_MONSTER_TURN, CoolTimeDecrease);
        EventManager<int>.StopListening(Define.ON_START_PLAYER_TURN, CardDraw);
        EventManager<bool>.StopListening(Define.ON_START_PLAYER_TURN, CardOnOff);
        EventManager<bool>.StopListening(Define.ON_START_MONSTER_TURN, CardOnOff);
        EventManager.StopListening(Define.ON_START_MONSTER_TURN, HandToDeck);
        transform.DOKill();
    }

    private Vector3 GetTouchPos()
    {
        Touch touch = Input.GetTouch(0);
        return Camera.main.ScreenToWorldPoint(touch.position);
    }
}