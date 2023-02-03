using DG.Tweening;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Windows;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using Input = UnityEngine.Input;
using SerializableDictionary;
using System;
using System.Linq;
using Unity.VisualScripting;
using Sequence = DG.Tweening.Sequence;

public enum RuneType
{
    Assist, // ï¿½ï¿½ï¿½ï¿½
    Main, // ï¿½ï¿½ï¿½ï¿½
}

[Serializable]
public class EffectPair
{
    public Condition Condition;
    public string Effect;
    public float Value;
}

[Serializable]
public class CustomDict : SerializableDictionary<EffectType, List<Pair>> { }

public class MagicCircle : MonoBehaviour, IPointerClickHandler
{
    private Dictionary<RuneType, List<Card>> _runeDict;
    public Dictionary<RuneType, List<Card>> RuneDict => _runeDict;

    //private Dictionary<string, string> _effectDict;
    //private Dictionary<EffectType, List<EffectPair>> _effectDict;
    [SerializeField]
    private CustomDict _effectDict;

    private const int _mainRuneCnt = 1;

    [SerializeField]
    private CardCollector _cardCollector;
    public CardCollector CardCollector => _cardCollector;

    [SerializeField]
    private float _assistRuneDistance = 3f;

    [SerializeField]
    private float _cardAreaDistance = 5f;
    public float CardAreaDistance => _cardAreaDistance;


    [SerializeField]
    private Card _runeTemplate;
    [SerializeField]
    private Card _garbageRuneTemplate;
    [SerializeField]
    private GameObject _bgPanel;
    [SerializeField]
    private Text _nameText;
    [SerializeField]
    private Text _effectText;
    [SerializeField]
    private MagicContent _effectContent;

    public Enemy enemy;
    private Vector2 touchBeganPos;
    private Vector2 touchEndedPos;
    private Vector2 touchDif;
    [SerializeField]
    private float swipeSensitivity;

    private bool _isBig = false;

    public bool IsBig
    {
        get => _isBig;
        set
        {
            if (_isBig == value) return;

            _isBig = value;

            if (_isBig)
            {
                // Å©ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
                transform.DOComplete();
                this.transform.DOScale(Vector3.one, 0.2f);
                //_bgPanel.GetComponent<Image>().DOFade(0.7f, 0.2f);
                _bgPanel.GetComponent<CanvasGroup>().DOFade(1f, 0.2f);
                this.transform.DOLocalMoveY(400, 0.2f).SetRelative();
                _bgPanel.transform.GetChild(0).GetComponent<Image>().raycastTarget = true;
                _effectContent.SetActive(true);
            }
            else
            {
                // ï¿½ï¿½ ï¿½ï¿½ï¿½â¸¸ Å¬ï¿½ï¿½ï¿½Ø¾ßµÇ´Â°ï¿½ ï¿½Æ´ï¿½
                // ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ Å¬ï¿½ï¿½ ï¿½ï¿½ Ä¿ï¿½ï¿½

                // Ä«ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ Ä¿ï¿½ï¿½

                // Ä«ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½Û¾ï¿½ï¿½ï¿½

                // ï¿½Û°ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿?
                transform.DOComplete();
                this.transform.DOScale(new Vector3(0.3f, 0.3f, 1), 0.2f);
                //_bgPanel.GetComponent<Image>().DOFade(0, 0.2f);
                _bgPanel.GetComponent<CanvasGroup>().DOFade(0, 0.2f);
                this.transform.DOLocalMoveY(-400, 0.2f).SetRelative();
                _bgPanel.transform.GetChild(0).GetComponent<Image>().raycastTarget = false;
                _effectContent.SetActive(false);
                //_bgPanel.transform.GetChild(0).GetComponent<Image>().raycastTarget = true;
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    public void Awake()
    {
        _runeDict = new Dictionary<RuneType, List<Card>>();
        _effectDict = new CustomDict();

        _nameText.text = "";
        _effectText.text = "";
    }

    private void Update()
    {
        //if (IsBig == true)
        //{
        Swipe1();
        //}
    }

    public void SortCard()
    {
        //transform.DOComplete();

        if (_runeDict.ContainsKey(RuneType.Assist))
        {
            float angle = -2 * Mathf.PI / _runeDict[RuneType.Assist].Count;

            for (int i = 0; i < _runeDict[RuneType.Assist].Count; i++)
            {
                //_runeDict[RuneType.Assist][i].GetComponent<RectTransform>().transform.rotation = Quaternion.Euler(0, 0, -1 * angle * i + 90);
                //_runeDict[RuneType.Assist][i].GetComponent<RectTransform>().anchoredPosition = new Vector3(_assistRuneDistance, 0, 0);
                float height = Mathf.Sin(angle * i + (90 * Mathf.Deg2Rad)) * _assistRuneDistance;
                float width = Mathf.Cos(angle * i + (90 * Mathf.Deg2Rad)) * _assistRuneDistance;
                if (_runeDict[RuneType.Assist][i] != null)
                {
                    _runeDict[RuneType.Assist][i].GetComponent<RectTransform>().anchoredPosition = new Vector3(width, height, 0);
                }
            }
        }

        if (_runeDict.ContainsKey(RuneType.Main))
        {
            if (_runeDict[RuneType.Main].Count == 1)
            {
                _runeDict[RuneType.Main][0].GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                _runeDict[RuneType.Main][0].GetComponentInParent<RectTransform>().transform.rotation = Quaternion.identity;
            }
            else
            {
                float angle = -2 * Mathf.PI / _runeDict[RuneType.Main].Count;
                float distance = 100;
                for (int i = 0; i < _runeDict[RuneType.Main].Count; i++)
                {
                    float height = Mathf.Sin(angle * i + (90 * Mathf.Deg2Rad)) * distance;
                    float width = Mathf.Cos(angle * i + (90 * Mathf.Deg2Rad)) * distance;
                    _runeDict[RuneType.Main][i].GetComponent<RectTransform>().anchoredPosition = new Vector3(width, height, 0);
                }
            }
        }
    }

    public Card AddCard(Card card)
    {
        if (card == null) return null;

        if (_isBig == false) return null;

        if (_runeDict.ContainsKey(RuneType.Main) == false || (_runeDict[RuneType.Main].Count == 0))
        {
            if (!DummyCost.Instance.CanUseMainRune(card.Rune.MainRune.Cost))
            {
                Debug.Log("ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½Ï±ï¿?ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½Õ´Ï´ï¿½.");

                return null;
            }
            if (_runeDict.ContainsKey(RuneType.Main))
            {
                if (_runeDict[RuneType.Main].Count >= _mainRuneCnt)
                {
                    Debug.Log("ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½Ö½ï¿½ï¿½Ï´ï¿½.");
                    return null;
                }

                Sequence seq = DOTween.Sequence();
                seq.AppendCallback(() =>
                {
                    card.GetComponent<RectTransform>().anchoredPosition = new Vector2(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y - _cardCollector.GetComponent<RectTransform>().anchoredPosition.y);
                    card.transform.SetParent(this.transform);
                    card.GetComponent<RectTransform>().anchoredPosition = card.GetComponent<RectTransform>().anchoredPosition;
                    card.GetComponent<RectTransform>().DOAnchorPos(Vector2.zero, 0.3f);
                    card.SetIsEquip(true);
                    card.SetCoolTime(card.Rune.MainRune.DelayTurn);
                    _cardCollector.CardRotate();
                });
                seq.AppendInterval(0.3f);
                seq.AppendCallback(() =>
                {
                    _runeDict[RuneType.Main].Add(card);
                    //for (int i = 0; i < _runeDict[RuneType.Main][0].Rune.AssistRuneCount; i++)
                    //{
                    //    GameObject ggo = Instantiate(_runeTemplate.gameObject, this.transform);
                    //    Card grune = ggo.GetComponent<Card>();
                    //    grune.SetRune(null);
                    //    grune.SetIsEquip(true);
                    //    if (_runeDict.ContainsKey(RuneType.Assist))
                    //    {
                    //        _runeDict[RuneType.Assist].Add(grune);
                    //    }
                    //    else
                    //    {
                    //        _runeDict.Add(RuneType.Assist, new List<Card> { grune });
                    //    }
                    //}

                    SortCard();
                    AddEffect(card, true);
                    AssistRuneAnimanation();
                    _effectContent.AddEffect(card.Rune.RuneEffect, true);
                });
                //SortCard();
            }
            else
            {
                Sequence seq = DOTween.Sequence();
                seq.AppendCallback(() =>
                {
                    card.GetComponent<RectTransform>().anchoredPosition = new Vector2(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y - _cardCollector.GetComponent<RectTransform>().anchoredPosition.y);
                    card.transform.SetParent(this.transform);
                    card.GetComponent<RectTransform>().anchoredPosition = card.GetComponent<RectTransform>().anchoredPosition;
                    card.GetComponent<RectTransform>().DOAnchorPos(Vector2.zero, 0.3f);
                    card.SetIsEquip(true);
                    card.SetCoolTime(card.Rune.MainRune.DelayTurn);
                    _cardCollector.CardRotate();
                });
                seq.AppendInterval(0.3f);
                seq.AppendCallback(() =>
                {
                    _runeDict.Add(RuneType.Main, new List<Card>() { card });
                    //for (int i = 0; i < _runeDict[RuneType.Main][0].Rune.AssistRuneCount; i++)
                    //{
                    //    GameObject ggo = Instantiate(_runeTemplate.gameObject, this.transform);
                    //    Card grune = ggo.GetComponent<Card>();
                    //    grune.SetRune(null);
                    //    grune.SetIsEquip(true);
                    //    if (_runeDict.ContainsKey(RuneType.Assist))
                    //    {
                    //        _runeDict[RuneType.Assist].Add(grune);
                    //    }
                    //    else
                    //    {
                    //        _runeDict.Add(RuneType.Assist, new List<Card> { grune });
                    //    }
                    //}

                    SortCard();
                    AddEffect(card, true);
                    AssistRuneAnimanation();
                    _effectContent.AddEffect(card.Rune.RuneEffect, true);
                    StartCoroutine(PlayEffect(card.Rune.RuneAudio));
                    Debug.Log("B");

                });
                //SortCard();
            }
        }
        else
        {
            if (!DummyCost.Instance.CanUseSubRune(card.Rune.AssistRune.Cost))
            {
                Debug.Log("ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½Ï±ï¿?ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½Õ´Ï´ï¿½.");
                return null;
            }

            if (_runeDict.ContainsKey(RuneType.Assist) == false)
            {
                //for (int i = 0; i < _runeDict[RuneType.Main][0].Rune.AssistRuneCount; i++)
                //{
                //    GameObject ggo = Instantiate(_runeTemplate.gameObject, this.transform);
                //    Card grune = ggo.GetComponent<Card>();
                //    grune.SetRune(null);
                //    grune.SetIsEquip(true);
                //    if (_runeDict.ContainsKey(RuneType.Assist))
                //    {
                //        _runeDict[RuneType.Assist].Add(grune);
                //    }
                //    else
                //    {
                //        _runeDict.Add(RuneType.Assist, new List<Card> { grune });
                //    }
                //}

                SortCard();
                AssistRuneAnimanation();
            }

            int changeIndex = -1;
            for (int i = 0; i < _runeDict[RuneType.Assist].Count; i++)
            {
                if (_runeDict[RuneType.Assist][i] != null && _runeDict[RuneType.Assist][i].Rune == null)
                {
                    changeIndex = i;
                    break;
                }
            }

            if (changeIndex == -1) return null;

            // ï¿½Ö´Ï¸ï¿½ï¿½Ì¼ï¿½ ï¿½Ú·ï¿½ ï¿½Ì·ï¿½ï¿?
            Sequence seq = DOTween.Sequence();
            seq.AppendCallback(() =>
            {
                card.GetComponent<RectTransform>().anchoredPosition = new Vector2(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y - _cardCollector.GetComponent<RectTransform>().anchoredPosition.y);
                card.transform.SetParent(this.transform);
                card.GetComponent<RectTransform>().anchoredPosition = card.GetComponent<RectTransform>().anchoredPosition;
                card.SetCoolTime(card.Rune.AssistRune.DelayTurn);
                card.SetIsEquip(true);
                if (_runeDict[RuneType.Assist][changeIndex] != null)
                {
                    card.GetComponent<RectTransform>().DOAnchorPos(_runeDict[RuneType.Assist][changeIndex].GetComponent<RectTransform>().anchoredPosition, 0.3f).OnComplete(() =>
                    {
                        Destroy(_runeDict[RuneType.Assist][changeIndex].gameObject);
                        _runeDict[RuneType.Assist][changeIndex] = card;

                        AddEffect(card, false);
                        //UpdateMagicName();
                        _effectContent.AddEffect(card.Rune.RuneEffect, false);
                    });
                }
            });
            seq.AppendCallback(() =>
            {
                SortCard();

                //UpdateMagicName();
                StartCoroutine(PlayEffect(card.Rune.RuneAudio));

            });
        }
        //SortCard();
        return card;
    }

private void AddEffect(Card card, bool main)
{
    if (card == null) return;

    if (main)
    {
        foreach (Pair e in card.Rune.MainRune.EffectDescription)
        {
            if (_effectDict.ContainsKey(e.EffectType))
            {

                _effectDict[e.EffectType].Add(e);
            }
            else
            {
                _effectDict.Add(e.EffectType, new List<Pair> { e });
            }
        }
    }
    else
    {
        foreach (Pair e in card.Rune.AssistRune.EffectDescription)
        {
            if (_effectDict.ContainsKey(e.EffectType))
            {

                _effectDict[e.EffectType].Add(e);
            }
            else
            {
                _effectDict.Add(e.EffectType, new List<Pair> { e });
            }
        }
    }
}

private void UpdateMagicName()
{
    if (_runeDict.ContainsKey(RuneType.Main))
    {
        if (_runeDict[RuneType.Main].Count > 0 && _runeDict[RuneType.Main][0].Rune != null)
        {
            #region Rune Name
            string name = "";
            for (int i = 0; i < _runeDict[RuneType.Assist].Count; i++)
            {
                if (_runeDict[RuneType.Assist][i].Rune != null)
                {
                    name += _runeDict[RuneType.Assist][i].Rune.AssistRune.Name + " ";

                    if (i == 2)
                    {
                        name += "\n";
                    }
                }
            }
            if (name == "")
            {
                name = _runeDict[RuneType.Main][0].Rune.MainRune.Name;
            }
            else
            {
                name = name.Substring(0, name.Length - 1);
                name += $"ÀÇ {_runeDict[RuneType.Main][0].Rune.MainRune.Name}";
            }
            _nameText.text = name;
            #endregion
        }
    }
}

public void AssistRuneAnimanation()
{
    SortCard();
    Sequence seq = DOTween.Sequence();
    //seq.AppendCallback(() => SortCard());
    seq.AppendCallback(() =>
    {
        if (_runeDict.ContainsKey(RuneType.Assist) == false)
        {
            for (int i = 0; i < _runeDict[RuneType.Main][0].Rune.AssistRuneCount; i++)
            {
                GameObject ggo = Instantiate(_runeTemplate.gameObject, this.transform);
                Card grune = ggo.GetComponent<Card>();
                grune.SetRune(null);
                grune.SetIsEquip(true);
                if (_runeDict.ContainsKey(RuneType.Assist))
                {
                    _runeDict[RuneType.Assist].Add(grune);
                }
                else
                {
                    _runeDict.Add(RuneType.Assist, new List<Card> { grune });
                }
            }

            SortCard();
        }
    });

    //seq.AppendInterval(0.1f);
    seq.AppendCallback(() =>
    {
        foreach (var r in _runeDict[RuneType.Assist])
        {
            if (r != null)
                r.GetComponent<RectTransform>().DOAnchorPos(Vector2.zero, 0.3f).From();
        }
    });
    //seq.AppendCallback(() => { UpdateMagicName(); });
    //seq.AppendInterval(0.2f);
    //seq.AppendCallback(() => { IsBig = false; });
}

public void Swipe1()
{
    if (Input.touchCount > 0)
    {
        Touch touch = Input.GetTouch(0);

        bool isSelectCard = false; // ÀÌ·± ½ÄÀÌ·Î ¸â¹ö º¯¼ö·Î •û±â
        if (touch.phase == TouchPhase.Began)
        {
            touchBeganPos = touch.position;
        }
        if (touch.phase == TouchPhase.Moved)
        {
            if (_cardCollector.SelectCard != null)
            {
                isSelectCard = true;
            }
        }
        if (touch.phase == TouchPhase.Ended)
        {
            touchEndedPos = touch.position;
            touchDif = (touchEndedPos - touchBeganPos);

            //ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½. ï¿½ï¿½Ä¡ï¿½ï¿½ xï¿½Ìµï¿½ï¿½Å¸ï¿½ï¿½ï¿½ yï¿½Ìµï¿½ï¿½Å¸ï¿½ï¿½ï¿½ ï¿½Î°ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ Å©ï¿½ï¿½
            if (Mathf.Abs(touchDif.y) > swipeSensitivity || Mathf.Abs(touchDif.x) > swipeSensitivity)
            {
                if (touchDif.y > 0 && Mathf.Abs(touchDif.y) > Mathf.Abs(touchDif.x))
                {
                    Debug.Log("up");
                }
                else if (touchDif.y < 0 && Mathf.Abs(touchDif.y) > Mathf.Abs(touchDif.x))
                {
                    Debug.Log("down");
                    // ¾Æ¹«Æ¾ ÀÏ´Ü ÇØ
                    if (_cardCollector.SelectCard == null && isSelectCard == false)
                    {
                        _cardCollector.CardRotate();
                    }
                }
                else if (touchDif.x > 0 && Mathf.Abs(touchDif.y) < Mathf.Abs(touchDif.x))
                {
                    Debug.Log("right");
                    if (IsBig == true)
                    {
                        if (touchBeganPos.y <= this.GetComponent<RectTransform>().anchoredPosition.y + this.GetComponent<RectTransform>().sizeDelta.y / 2
                            && touchBeganPos.y >= this.GetComponent<RectTransform>().anchoredPosition.y - this.GetComponent<RectTransform>().sizeDelta.y / 2)
                        {
                            Damage();
                        }
                    }
                }
                else if (touchDif.x < 0 && Mathf.Abs(touchDif.y) < Mathf.Abs(touchDif.x))
                {
                    Debug.Log("Left");
                }
            }
            //ï¿½ï¿½Ä¡.
            else
            {
                Debug.Log("touch");
            }
        }
    }
}


public void Damage() // ´ëÃË ¼öÁ¤
{
    // ï¿½×³ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½Â´Ù´ï¿?ï¿½ï¿½ï¿½ï¿½, ï¿½Ù¸ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½Ù°ï¿½ ï¿½Å±ï¿½ ï¿½ï¿½ï¿½ï¿½

    // ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ È¸ï¿½ï¿½ È¿ï¿½ï¿½
    // ï¿½ï¿½ï¿½Ì¿ï¿½ ï¿½ß°ï¿½ï¿½ï¿½ ï¿½Ù¸ï¿½ È¿ï¿½ï¿½ï¿½ï¿½ ï¿½Ö°ï¿½ï¿½ï¿½
    // ï¿½ï¿½ ï¿½Ä¿ï¿½ ï¿½ï¿½ï¿½ï¿½

    if (_runeDict.ContainsKey(RuneType.Main) == false || _runeDict[RuneType.Main].Count == 0 || _runeDict[RuneType.Main][0].Rune == null)
    {
        // ï¿½ï¿½ï¿½ï¿½ ï¿½ÈµÇ´ï¿½ ï¿½ï¿½ï¿½ï¿½Æ®
        // ï¿½ï¿½.. Ä«ï¿½Þ¶ï¿½ ï¿½ï¿½é¸²ï¿½Ì¶ï¿½ï¿½ï¿½ï¿½
        Debug.Log("ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½");
        return;
    }

    Sequence seq = DOTween.Sequence();
    seq.Append(this.transform.DORotate(new Vector3(0, 0, -360 * 5), 0.7f, RotateMode.LocalAxisAdd).SetEase(Ease.OutCubic));
    seq.InsertCallback(0f, () => _effectContent.AttackAnimation());

    //int damage = 0;
    seq.AppendInterval(0.1f);
    seq.AppendCallback(() =>
    {
        // ºÎ¿©, ¹æ¾î, °ø°Ý, »èÁ¦ ¼ø¼­
        for (int i = _runeDict[RuneType.Assist].Count - 1; i >= 0; i--)
        {
            if (_runeDict[RuneType.Assist][i].Rune == null)
            {
                Destroy(_runeDict[RuneType.Assist][i].gameObject);
                _runeDict[RuneType.Assist].RemoveAt(i);
            }
        }


        AttackFunction(EffectType.Status);
        AttackFunction(EffectType.Defence);
        AttackFunction(EffectType.Attack);
        AttackFunction(EffectType.Draw);
        AttackFunction(EffectType.Destroy);
    });
    seq.AppendCallback(() =>
    {
        Debug.Log("Attack Complate");
        foreach (var item in _runeDict)
        {
            foreach (Card card in item.Value)
            {
                _cardCollector._restCards.Add(card);
            }
        }
        _cardCollector.UIUpdate();

        foreach (var rList in _runeDict)
        {
            foreach (var r in rList.Value)
            {
                if (r.Rune == null)
                {
                    Destroy(r.gameObject);
                }
                else
                {
                    r.gameObject.SetActive(false);
                    r.transform.SetParent(_cardCollector.transform);
                    r.SetIsEquip(false);
                }
            }
        }

        _runeDict.Clear();
        _effectDict.Clear();
        _nameText.text = "";
        _effectText.text = "";
        _effectContent.Clear();

        IsBig = false;

        if (_cardCollector.IsFront == false)
        {
            _cardCollector.CardRotate();
        }
    });

    //enemy.Damage(damage);
}

public void AttackFunction(EffectType effectType)
{
    Action action = null;
    if (_effectDict.ContainsKey(effectType))
    {
        foreach (var e in _effectDict[effectType])
        {
            Unit target = e.IsEnemy == true ? GameManager.Instance.enemy : GameManager.Instance.player;

            int c = 0;
            for (int i = 0; i < _runeDict[RuneType.Assist].Count; i++)
            {
                if (_runeDict[RuneType.Assist][i].Rune != null && _runeDict[RuneType.Assist][i].Rune.AssistRune.Attribute == e.AttributeType)
                {
                    c++;
                }
            }
            if (_runeDict[RuneType.Main][0].Rune != null && _runeDict[RuneType.Main][0].Rune.AssistRune.Attribute == e.AttributeType)
                c++;

            switch (effectType)
            {
                case EffectType.Attack:
                    switch (e.AttackType)
                    {
                        case AttackType.Single:
                            action = () => GameManager.Instance.player.Attack(int.Parse(e.Effect));
                            break;
                        case AttackType.Double:
                            action = () => GameManager.Instance.player.Attack(int.Parse(e.Effect) * c);
                            break;
                    }
                    break;
                case EffectType.Defence:
                    switch (e.AttackType)
                    {
                        case AttackType.Single:
                            action = () => GameManager.Instance.player.Shield += int.Parse(e.Effect);
                            break;
                        case AttackType.Double:
                            action = () => GameManager.Instance.player.Shield += int.Parse(e.Effect) * c;
                            break;
                    }
                    break;
                case EffectType.Status:
                    switch (e.AttackType)
                    {
                        case AttackType.Single:
                            action = () => StatusManager.Instance.AddStatus(target, e.StatusType, int.Parse(e.Effect));
                            break;
                        case AttackType.Double:
                            action = () => StatusManager.Instance.AddStatus(target, e.StatusType, int.Parse(e.Effect) * c);
                            break;
                    }
                    break;
                case EffectType.Destroy:
                    action = () => StatusManager.Instance.RemStatus(target, e.StatusType);
                    break;
                case EffectType.Draw:
                    _cardCollector.CardDraw(int.Parse(e.Effect));
                    break;
                case EffectType.Etc:
                    break;
            }

            switch (e.Condition.ConditionType)
            {
                case ConditionType.None:
                    action?.Invoke();
                    break;
                case ConditionType.HeathComparison:
                    if (target.IsHealthAmount(e.Condition.Value, e.Condition.ComparisonType))
                    {
                        action?.Invoke();
                    }
                    break;
                case ConditionType.AssistRuneCount:
                    int count = 0;
                    for (int i = 0; i < _runeDict[RuneType.Assist].Count; i++)
                    {
                        if (_runeDict[RuneType.Assist][i].Rune != null)
                        {
                            count++;
                        }
                    }
                    if (count >= e.Condition.Value)
                    {
                        action?.Invoke();
                    }
                    break;
                case ConditionType.AttributeComparison:
                    int cnt = 0;
                    if (_runeDict[RuneType.Main][0].Rune.MainRune.Attribute == e.Condition.AttributeType)
                        cnt++;
                    for (int i = 0; i < _runeDict[RuneType.Assist].Count; i++)
                    {
                        if (_runeDict[RuneType.Assist][i].Rune.AssistRune.Attribute == e.Condition.AttributeType)
                        {
                            cnt++;
                        }
                    }

                    switch (e.Condition.ComparisonType)
                    {
                        case ComparisonType.MoreThan:
                            if (cnt >= e.Condition.Value)
                            {
                                action?.Invoke();
                            }
                            break;
                        case ComparisonType.LessThan:
                            if (cnt <= e.Condition.Value)
                            {
                                action?.Invoke();
                            }
                            break;
                    }
                    break;
                case ConditionType.StatusComparison:
                    switch (e.Condition.ComparisonType)
                    {
                        case ComparisonType.MoreThan:
                            if (StatusManager.Instance.GetUnitStatusValue(target, e.Condition.StatusType) >= e.Condition.Value)
                            {
                                action?.Invoke();
                            }
                            break;
                        case ComparisonType.LessThan:
                            if (StatusManager.Instance.GetUnitStatusValue(target, e.Condition.StatusType) <= e.Condition.Value)
                            {
                                action?.Invoke();
                            }
                            break;
                    }
                    break;
            }
        }
    }
}

public void OnDestroy()
{
    transform.DOKill();
}

public void OnPointerClick(PointerEventData eventData)
{
    if (IsBig == false)
    {
        IsBig = true;
    }
}

private IEnumerator PlayEffect(AudioClip clip)
{
    SoundManager.Instance.PlaySound(clip, SoundType.Effect);
    yield return new WaitForSeconds(1.5f);
    SoundManager.Instance.StopSound(SoundType.Effect);
}


#if UNITY_EDITOR
private void OnDrawGizmosSelected()
{
    Gizmos.color = Color.green;
    Gizmos.DrawWireSphere(transform.position, _cardAreaDistance);
    Gizmos.color = Color.white;
}
#endif
}
