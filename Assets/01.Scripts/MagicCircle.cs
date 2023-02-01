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
                _bgPanel.GetComponent<CanvasGroup>().DOFade(0.7f, 0.2f);
                this.transform.DOLocalMoveY(400, 0.2f).SetRelative();
                _bgPanel.transform.GetChild(0).GetComponent<Image>().raycastTarget = true;
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
                _runeDict[RuneType.Assist][i].GetComponent<RectTransform>().anchoredPosition = new Vector3(width, height, 0);
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
                    GameObject g = Instantiate(_garbageRuneTemplate.gameObject, this.transform);
                    card.GetComponent<RectTransform>().anchoredPosition = new Vector2(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y - _cardCollector.GetComponent<RectTransform>().anchoredPosition.y);
                    card.transform.SetParent(this.transform);
                    g.GetComponent<RectTransform>().anchoredPosition = card.GetComponent<RectTransform>().anchoredPosition;
                    g.GetComponent<RectTransform>().DOAnchorPos(GetComponent<RectTransform>().anchoredPosition, 0.3f).OnComplete(() =>
                    {
                        Destroy(g);

                        GameObject go = Instantiate(_runeTemplate.gameObject, this.transform);
                        Card rune = go.GetComponent<Card>();
                        rune.SetRune(card.Rune);
                        rune.SetIsEquip(true);
                        card.SetCoolTime(card.Rune.MainRune.DelayTurn);
                        card.SetIsEquip(true);
                        _runeDict[RuneType.Main].Add(card);

                        for (int i = 0; i < _runeDict[RuneType.Main][0].Rune.AssistRuneCount; i++)
                        {
                            GameObject ggo = Instantiate(_runeTemplate.gameObject, this.transform);
                            Card grune = ggo.GetComponent<Card>();
                            //grune.SetRune(null);
                            //grune.SetIsEquip(true);
                            //grune.CardAnimation();
                            if (_runeDict.ContainsKey(RuneType.Assist))
                            {
                                _runeDict[RuneType.Assist].Add(grune);
                            }
                            else
                            {
                                _runeDict.Add(RuneType.Assist, new List<Card> { grune });
                            }
                        }
                        _cardCollector.CardRotate();
                        SortCard();
                        AddEffect(card, true);
                        AssistRuneAnimanation();
                    });
                });
                SortCard();
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
                    for (int i = 0; i < _runeDict[RuneType.Main][0].Rune.AssistRuneCount; i++)
                    {
                        GameObject ggo = Instantiate(_runeTemplate.gameObject, this.transform);
                        Card grune = ggo.GetComponent<Card>();
                        grune.SetRune(null);
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
                    AddEffect(card, true);
                    AssistRuneAnimanation();
                });
                SortCard();
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
                AssistRuneAnimanation();
            }

            int changeIndex = -1;
            for (int i = 0; i < _runeDict[RuneType.Assist].Count; i++)
            {
                if (_runeDict[RuneType.Assist][i].Rune == null)
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
                card.GetComponent<RectTransform>().DOAnchorPos(_runeDict[RuneType.Assist][changeIndex].GetComponent<RectTransform>().anchoredPosition, 0.3f).OnComplete(() =>
                {
                    Destroy(_runeDict[RuneType.Assist][changeIndex].gameObject);

                    _runeDict[RuneType.Assist][changeIndex] = card;

                    AddEffect(card, false);
                    UpdateMagicName();
                });
            });
            SortCard();
        }

        SortCard();
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

                string effect = "";
                //foreach(var e in _effectDict)
                //{
                //    switch (e.Key)
                //    {
                //        case "Attack":
                //            effect += e.Value + "µ¥¹ÌÁö ";
                //            break;
                //        case "Defence":
                //            effect += e.Value + "¹æ¾îµµ ";
                //            break;
                //        case "Weak":
                //            effect += "¾àÈ­ " + e.Value + "ºÎ¿© ";
                //            break;
                //        case "Fire":
                //            effect += "È­»ó " + e.Value + "ºÎ¿© ";
                //            break;
                //        case "Ice":
                //            effect += "ºù°á " + e.Value + "ºÎ¿© ";
                //            break;
                //        default:
                //            break;
                //    }
                //}
                //effect = effect.Substring(0, effect.Length - 1);
                _effectText.text = effect;
            }
        }
    }

    public void AssistRuneAnimanation()
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() => SortCard());
        foreach (var r in _runeDict[RuneType.Assist])
        {
            seq.Join(r.GetComponent<RectTransform>().DOAnchorPos(Vector2.zero, 0.3f).From());
        }
        seq.AppendCallback(() => { UpdateMagicName(); });
        //seq.AppendInterval(0.2f);
        //seq.AppendCallback(() => { IsBig = false; });
    }

    public void Swipe1()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            bool touchStart = false;
            bool touchEnd = false;

            if (touch.phase == TouchPhase.Began)
            {
                touchBeganPos = touch.position;
                if (_cardCollector.SelectCard == null)
                {
                    touchStart = false;
                }
                else
                {
                    touchStart = true;
                }
            }
            if (touch.phase == TouchPhase.Ended)
            {
                touchEndedPos = touch.position;
                touchDif = (touchEndedPos - touchBeganPos);
                if (_cardCollector.SelectCard == null)
                {
                    touchEnd = false;
                }
                else
                {
                    touchEnd = true;
                }

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
                        if (touchStart == false && touchEnd == false && _cardCollector.SelectCard == null)
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

        //int damage = 0;
        seq.AppendInterval(0.1f);
        seq.AppendCallback(() =>
        {
            // ºÎ¿©, ¹æ¾î, °ø°Ý, »èÁ¦ ¼ø¼­

            //foreach (var e in _effectDict)
            //{
            //    switch (e.Key)
            //    {
            //        case "Defence":
            //            // ¾ÆÁ÷ ¾øÀ½
            //            break;
            //        case "Weak":
            //            StatusManager.Instance.AddStatus(GameManager.Instance.enemy, StatusName.Weak, int.Parse(e.Value));
            //            break;
            //        case "Fire":
            //            StatusManager.Instance.AddStatus(GameManager.Instance.enemy, StatusName.Fire, int.Parse(e.Value));
            //            break;
            //        case "Ice":
            //            StatusManager.Instance.AddStatus(GameManager.Instance.enemy, StatusName.Ice, int.Parse(e.Value));
            //            break;
            //        default:
            //            break;
            //    }
            //}

            //foreach (var e in _effectDict)
            //{
            //    switch (e.Key)
            //    {
            //        case "Attack":
            //            GameManager.Instance.player.Attack(int.Parse(e.Value));
            //            break;
            //    }
            //}

            foreach (var e in _effectDict[EffectType.Status])
            {
                Unit target = e.IsEnemy == true ? GameManager.Instance.enemy : GameManager.Instance.player;
                switch (e.Condition.ConditionType)
                {
                    case ConditionType.None:
                        StatusManager.Instance.AddStatus(target, e.StatusType, int.Parse(e.Effect));
                        break;
                    case ConditionType.IfThereIs:
                        if(e.Condition.AttributeType != AttributeType.None)
                        {
                            bool b = false;
                            if (_runeDict[RuneType.Main][0].Rune.MainRune.Attribute == e.Condition.AttributeType)
                                b = true;
                            for(int i = 0; i < _runeDict[RuneType.Assist].Count; i++)
                            {
                                if (_runeDict[RuneType.Assist][i].Rune.AssistRune.Attribute == e.Condition.AttributeType)
                                    b = true;
                            }

                            if(b == true)
                            {
                                StatusManager.Instance.AddStatus(target, e.StatusType, int.Parse(e.Effect));
                            }
                        }
                        else if(e.Condition.StatusType != StatusName.Null)
                        {
                            if (StatusManager.Instance.IsHaveStatus(target, e.Condition.StatusType) == true)
                            {
                                StatusManager.Instance.AddStatus(target, e.StatusType, int.Parse(e.Effect));
                            }
                        }
                        break;
                    case ConditionType.IfNotThereIs:
                        if (e.Condition.AttributeType != AttributeType.None)
                        {
                            bool b = false;
                            if (_runeDict[RuneType.Main][0].Rune.MainRune.Attribute == e.Condition.AttributeType)
                                b = true;
                            for (int i = 0; i < _runeDict[RuneType.Assist].Count; i++)
                            {
                                if (_runeDict[RuneType.Assist][i].Rune.AssistRune.Attribute == e.Condition.AttributeType)
                                {
                                    b = true;
                                    break;
                                }
                            }

                            if (b == false)
                            {
                                StatusManager.Instance.AddStatus(target, e.StatusType, int.Parse(e.Effect));
                            }
                        }
                        else if (e.Condition.StatusType != StatusName.Null)
                        {
                            if (StatusManager.Instance.IsHaveStatus(target, e.Condition.StatusType) == false)
                            {
                                StatusManager.Instance.AddStatus(target, e.StatusType, int.Parse(e.Effect));
                            }
                        }
                        break;
                    case ConditionType.Heath:
                        if(e.Condition.StatusType == StatusName.Null) // ÇÇ
                        {
                            if(target.IsHealthAmount(e.Condition.Value, e.Condition.HeathType))
                            {
                                StatusManager.Instance.AddStatus(target, e.StatusType, int.Parse(e.Effect));
                            }
                        }
                        else // ½ºÅÈ
                        {

                        }
                        break;
                    case ConditionType.AssistRuneCount:
                        int count = 0;
                        for(int i = 0; i < _runeDict[RuneType.Assist].Count; i++)
                        {
                            if (_runeDict[RuneType.Assist][i].Rune != null)
                            {
                                count++;
                            }
                        }
                        if(count >= e.Condition.Value)
                        {
                            StatusManager.Instance.AddStatus(target, e.StatusType, int.Parse(e.Effect));
                        }
                        break;
                }
            }
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
                    }
                }
            }

            _runeDict.Clear();
            _effectDict.Clear();
            _nameText.text = "";
            _effectText.text = "";

            IsBig = false;

            if (_cardCollector.IsFront == false)
            {
                _cardCollector.CardRotate();
            }
        });

        //enemy.Damage(damage);
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

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _cardAreaDistance);
        Gizmos.color = Color.white;
    }
#endif
}
