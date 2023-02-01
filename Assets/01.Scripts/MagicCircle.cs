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

public enum RuneType
{
    Assist, // ����
    Main, // ����
}

public class MagicCircle : MonoBehaviour, IPointerClickHandler
{

    private Dictionary<RuneType, List<Card>> _runeDict;
    public Dictionary<RuneType, List<Card>> RuneDict => _runeDict;

    private Dictionary<string, string> _effectDict;

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
                // ũ�� ����
                transform.DOComplete();
                this.transform.DOScale(Vector3.one, 0.2f);
                //_bgPanel.GetComponent<Image>().DOFade(0.7f, 0.2f);
                _bgPanel.GetComponent<CanvasGroup>().DOFade(0.7f, 0.2f);
                this.transform.DOLocalMoveY(400, 0.2f).SetRelative();
                _bgPanel.transform.GetChild(0).GetComponent<Image>().raycastTarget = true;
            }
            else
            {
                // �� ���⸸ Ŭ���ؾߵǴ°� �ƴ�
                // ������ Ŭ�� �� Ŀ��

                // ī�� ���� �� Ŀ��

                // ī�� ������ �۾���

                // �۰� �����?
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
        _effectDict = new Dictionary<string, string>();

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
            if(_runeDict[RuneType.Main].Count == 1)
            {
                _runeDict[RuneType.Main][0].GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                _runeDict[RuneType.Main][0].GetComponentInParent<RectTransform>().transform.rotation = Quaternion.identity;
            }
            else
            {
                float angle = -2 * Mathf.PI / _runeDict[RuneType.Main].Count;
                float distance = 100;
                for(int i = 0; i < _runeDict[RuneType.Main].Count; i++)
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
                Debug.Log("���� ���� ����ϱ�?���� ������ �����մϴ�.");

                return null;
            }
            if (_runeDict.ContainsKey(RuneType.Main))
            {
                if (_runeDict[RuneType.Main].Count >= _mainRuneCnt)
                {
                    Debug.Log("���� ���� ���� �ֽ��ϴ�.");
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
                Debug.Log("���� ���� ����ϱ�?���� ������ �����մϴ�.");
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

            // �ִϸ��̼� �ڷ� �̷��?
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
            PairListFunction(card.Rune.MainRune.EffectDescription);
        }
        else
        {
            PairListFunction(card.Rune.AssistRune.EffectDescription);
        }
    }

    private void PairListFunction(List<Pair> list)
    {
        foreach (var e in list)
        {
            if (e.Condition.ConditionType == ConditionType.None)
            {
                AddEffectDict(e);
            }
            else
            {
                switch (e.Condition.ConditionType)
                {
                    case ConditionType.IfTherIs:
                        if(e.Condition.AttributeType != AttributeType.None)
                        {
                            if(e.Condition.IsEnemyOrMain == true)
                            {
                                if (_runeDict[RuneType.Main][0].Rune.MainRune.Attribute == e.Condition.AttributeType)
                                {
                                    AddEffectDict(e);
                                }
                            }
                            else
                            {
                                bool isAttribute = false;
                                foreach(var a in _runeDict[RuneType.Assist])
                                {
                                    if(a.Rune.AssistRune.Attribute == e.Condition.AttributeType)
                                    {
                                        isAttribute = true;
                                        break;
                                    }
                                }

                                if(isAttribute == true)
                                {
                                    AddEffectDict(e);
                                }
                            }
                        }
                        else if(e.Condition.StatusType != StatusName.Null)
                        {
                            if (e.Condition.IsEnemyOrMain == true)
                            {
                                if(StatusManager.Instance.IsHaveStatus(GameManager.Instance.enemy, e.Condition.StatusType))
                                {
                                    AddEffectDict(e);
                                }
                            }
                            else
                            {
                                if (StatusManager.Instance.IsHaveStatus(GameManager.Instance.player, e.Condition.StatusType))
                                {
                                    AddEffectDict(e);
                                }
                            }
                        }
                        break;
                    case ConditionType.Heath:
                        if(e.Condition.StatusType != StatusName.Null)
                        {
                            if(e.Condition.IsEnemyOrMain == true)
                            {
                                switch (e.Condition.HeathType)
                                {
                                    case HealthType.MoreThan:
                                        // �� StatusType �� �����̻��� value �̻��϶�
                                        break;
                                    case HealthType.LessThan:
                                        // ����
                                        break;
                                    case HealthType.Percentage:
                                        // �ۼ�Ʈ
                                        break;
                                }
                            }
                            else
                            {
                                switch (e.Condition.HeathType)
                                {
                                    case HealthType.MoreThan:
                                        // �� StatusType �� �����̻��� value �̻��϶�
                                        break;
                                    case HealthType.LessThan:
                                        // ����
                                        break;
                                    case HealthType.Percentage:
                                        // �ۼ�Ʈ
                                        break;
                                }
                            }
                        }
                        else
                        {
                            if (e.Condition.IsEnemyOrMain == true)
                            {
                                switch (e.Condition.HeathType)
                                {
                                    case HealthType.MoreThan:
                                        // �� hp �� �����̻��� value �̻��϶�
                                        break;
                                    case HealthType.LessThan:
                                        // ����
                                        break;
                                    case HealthType.Percentage:
                                        // �ۼ�Ʈ
                                        break;
                                }
                            }
                            else
                            {
                                switch (e.Condition.HeathType)
                                {
                                    case HealthType.MoreThan:
                                        // �� hp �� �����̻��� value �̻��϶�
                                        break;
                                    case HealthType.LessThan:
                                        // ����
                                        break;
                                    case HealthType.Percentage:
                                        // �ۼ�Ʈ
                                        break;
                                }
                            }
                        }
                        break;
                    case ConditionType.AssistRuneCount:
                        int count = 0;
                        foreach(var r in _runeDict[RuneType.Assist])
                        {
                            if (r.Rune != null)
                                count++;
                        }
                        if(count >= e.Condition.Value)
                        {
                            AddEffectDict(e);
                        }
                        break;
                }
            }
        }
    }

    private void AddEffectDict(Pair e)
    {
        if (e.EffectType == EffectType.Status)
        {
            string effect = Enum.GetName(typeof(StatusName), e.StatusType);
            if (_effectDict.ContainsKey(effect))
            {
                _effectDict[effect] = (int.Parse(e.Effect) + int.Parse(_effectDict[effect])).ToString();
            }
            else
            {
                _effectDict.Add(effect, e.Effect);
            }
        }
        else if (e.EffectType == EffectType.Etc)
        {
            string effect = e.Effect;
            if (_effectDict.ContainsKey(effect))
            {
                //_effectDict[effect] += e.Effect;
            }
            else
            {
                _effectDict.Add(effect, effect);
            }
        }
        else
        {
            string effect = Enum.GetName(typeof(EffectType), e.EffectType);
            if (_effectDict.ContainsKey(effect))
            {
                _effectDict[effect] = (int.Parse(e.Effect) + int.Parse(_effectDict[effect])).ToString();
            }
            else
            {
                _effectDict.Add(effect, e.Effect);
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
                    name += $"�� {_runeDict[RuneType.Main][0].Rune.MainRune.Name}";
                }
                _nameText.text = name;
                #endregion

                string effect = "";
                foreach(var e in _effectDict)
                {
                    switch (e.Key)
                    {
                        case "Attack":
                            effect += e.Value + "������ ";
                            break;
                        case "Defence":
                            effect += e.Value + "�� ";
                            break;
                        case "Weak":
                            effect += "��ȭ " + e.Value + "�ο� ";
                            break;
                        case "Fire":
                            effect += "ȭ�� " + e.Value + "�ο� ";
                            break;
                        case "Ice":
                            effect += "���� " + e.Value + "�ο� ";
                            break;
                        default:
                            break;
                    }
                }
                effect = effect.Substring(0, effect.Length - 1);
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
                if(_cardCollector.SelectCard == null)
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

                //��������. ��ġ�� x�̵��Ÿ��� y�̵��Ÿ��� �ΰ������� ũ��
                if (Mathf.Abs(touchDif.y) > swipeSensitivity || Mathf.Abs(touchDif.x) > swipeSensitivity)
                {
                    if (touchDif.y > 0 && Mathf.Abs(touchDif.y) > Mathf.Abs(touchDif.x))
                    {
                        Debug.Log("up");
                    }
                    else if (touchDif.y < 0 && Mathf.Abs(touchDif.y) > Mathf.Abs(touchDif.x))
                    {
                        Debug.Log("down");
                        // �ƹ�ƾ �ϴ� ��
                        if(touchStart == false && touchEnd == false && _cardCollector.SelectCard == null)
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
                //��ġ.
                else
                {
                    Debug.Log("touch");
                }
            }
        }
    }


    public void Damage()
    {
        // �׳� ������ ������´ٴ�?����, �ٸ� �����ٰ� �ű� ����

        // ������ ȸ�� ȿ��
        // ���̿� �߰��� �ٸ� ȿ���� �ְ���
        // �� �Ŀ� ����

        if (_runeDict.ContainsKey(RuneType.Main) == false || _runeDict[RuneType.Main].Count == 0 || _runeDict[RuneType.Main][0].Rune == null)
        {
            // ���� �ȵǴ� ����Ʈ
            // ��.. ī�޶� ��鸲�̶����
            Debug.Log("���� �� ����");
            return;
        }

        Sequence seq = DOTween.Sequence();
        seq.Append(this.transform.DORotate(new Vector3(0, 0, -360 * 5), 0.7f, RotateMode.LocalAxisAdd).SetEase(Ease.OutCubic));

        //int damage = 0;
        seq.AppendInterval(0.1f);
        seq.AppendCallback(() =>
        {
            foreach (var e in _effectDict)
            {
                switch (e.Key)
                {
                    case "Defence":
                        // ���� ����
                        break;
                    case "Weak":
                        StatusManager.Instance.AddStatus(GameManager.Instance.enemy, StatusName.Weak, int.Parse(e.Value));
                        break;
                    case "Fire":
                        StatusManager.Instance.AddStatus(GameManager.Instance.enemy, StatusName.Fire, int.Parse(e.Value));
                        break;
                    case "Ice":
                        StatusManager.Instance.AddStatus(GameManager.Instance.enemy, StatusName.Ice, int.Parse(e.Value));
                        break;
                    default:
                        break;
                }
            }

            foreach (var e in _effectDict)
            {
                switch (e.Key)
                {
                    case "Attack":
                        GameManager.Instance.player.Attack(int.Parse(e.Value));
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

            foreach(var rList in _runeDict)
            {
                foreach(var r in rList.Value)
                {
                    if(r.Rune == null)
                    {
                        Destroy(r.gameObject);
                        // ī����� destroy��Ű�� �ȵȴ�.

                        // ���⼭ �������� �ִ� ����� rest�� �ȱ�� �� ��
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

            if(_cardCollector.IsFront == false)
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
