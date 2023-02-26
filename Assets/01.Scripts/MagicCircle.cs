using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Input = UnityEngine.Input;
using SerializableDictionary;
using System;
using System.Linq;
using Sequence = DG.Tweening.Sequence;

public enum RuneType
{
    Assist, // ����
    Main, // ����
}

[Serializable]
public class EffectPair
{
    public Condition Condition;
    public string Effect;
    public float Value;
}

[Serializable]
public class EffectObjectPair
{
    public Pair pair;
    public GameObject effect;

    public EffectObjectPair(Pair pair, GameObject effect)
    {
        this.pair = pair;
        this.effect = effect;
    }
}

[Serializable]
public class CustomDict : SerializableDictionary<EffectType, List<EffectObjectPair>> { }

[Serializable]
public class CustomRuneDict : SerializableDictionary<RuneType, List<Card>> { }

public class MagicCircle : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private CustomRuneDict _runeDict;
    public CustomRuneDict RuneDict => _runeDict;
    private Dictionary<RuneType, List<Card>> _runeTempDict;
    public Dictionary<RuneType, List<Card>> RuneTempDict => _runeTempDict;

    //private Dictionary<string, string> _effectDict;
    //private Dictionary<EffectType, List<EffectPair>> _effectDict;
    [SerializeField]
    private CustomDict _effectDict;
    public CustomDict EffectDict => _effectDict;

    private Dictionary<EffectType, List<EffectObjectPair>> _tempEffectDict;
    public Dictionary<EffectType, List<EffectObjectPair>> TempEffectDict => _tempEffectDict;

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
    public MagicContent EffectContent => _effectContent;

    public Enemy enemy;
    private Vector2 touchBeganPos;
    private Vector2 touchEndedPos;
    private Vector2 touchDif;
    [SerializeField]
    private float swipeSensitivity;

    [Header("Sounds")]
    [SerializeField]
    private AudioClip clickSound = null;
    [SerializeField]
    private AudioClip attackSound = null;

    private bool _isAddCard = false;
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
                _bgPanel.GetComponent<CanvasGroup>().DOFade(1f, 0.2f);
                this.transform.DOLocalMoveY(400, 0.2f).SetRelative();
                _bgPanel.transform.GetChild(0).GetComponent<Image>().raycastTarget = true;
                _effectContent.SetActive(true);
                SoundManager.Instance.PlaySound(clickSound, SoundType.Effect);
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
                _effectContent.SetActive(false);
                //_bgPanel.transform.GetChild(0).GetComponent<Image>().raycastTarget = true;
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
                _cardCollector.HandCardSetRune(false);
            }
        }
    }

    public void Awake()
    {
        _runeDict = new CustomRuneDict();
        _effectDict = new CustomDict();

        _nameText.text = "";
        _effectText.text = "";

    }

    private void Update()
    {
        Swipe1();

        //_rotateAxis = (_rotateAxis + _rotateSpeed) % 360f;
        //transform.rotation = Quaternion.Euler(0f, 0f, _rotateAxis);
    }

    public void SortCard()
    {
        //transform.DOComplete();

        if (_runeDict.ContainsKey(RuneType.Assist))
        {
            float angle = -2 * Mathf.PI / _runeDict[RuneType.Assist].Count;

            for (int i = 0; i < _runeDict[RuneType.Assist].Count; i++)
            {
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

        if (_isAddCard == true) return null;

        if (_runeDict.ContainsKey(RuneType.Main) == false || (_runeDict[RuneType.Main].Count == 0))
        {
            Touch touch = Input.GetTouch(0);
            Vector3 pos = Camera.main.ScreenToWorldPoint(touch.position);

            if (!DummyCost.Instance.CanRune(card.Rune.MainRune.Cost))
            {
                UIManager.Instance.InfoMessagePopup("������ �����մϴ�.", pos);
                return null;
            }
            //if (_runeDict[RuneType.Main].Count >= _mainRuneCnt)
            //{
            //    UIManager.Instance.InfoMessagePopup("���� ���� ���� �־��ּ���.", pos);
            //    return null;
            //}

            Sequence seq = DOTween.Sequence();
            seq.AppendCallback(() =>
            {
                _isAddCard = true;
                //card.GetComponent<RectTransform>().anchoredPosition = Input.GetTouch(0).position;
                card.transform.SetParent(this.transform); // �ߵ� �ȵ�
                DummyCost.Instance.CanUseMainRune(card.Rune.MainRune.Cost);
                //card.GetComponent<RectTransform>().anchoredPosition = card.GetComponent<RectTransform>().anchoredPosition;
                if (_runeDict.ContainsKey(RuneType.Main))
                {
                    _runeDict[RuneType.Main].Add(card);
                }
                else
                {
                    _runeDict.Add(RuneType.Main, new List<Card>() { card }); // �ߵ� ��
                }
                card.SetIsEquip(true); // �ߵ� �ȵ�
                card.GetComponent<RectTransform>().DOAnchorPos(GetComponent<RectTransform>().anchoredPosition, 0.3f).OnComplete(() =>
                {
                    card.SetOutlineActive(true);
                    SortCard();
                    AddEffect(card, true); // �ߵ� �ǳ�?
                    AssistRuneAnimanation(); // �ϴٰ� �����
                    _effectContent.AddEffect(card.Rune, true); // ��
                                                               //_cardCollector.CardRotate();
                    _cardCollector.IsFront = false; // �Ʒ��� ��
                    _cardCollector.CardSort();
                    StartCoroutine(PlayEffect(card.Rune.RuneAudio));
                    _isAddCard = false;
                    Debug.Log("B");
                }); // �߿� ������ �ڿ��Ŵ� ��
                                       //card.SetCoolTime(card.Rune.MainRune.DelayTurn); // �ߵ� ��
                _cardCollector.CardRotate(); // �ߵ� ��
            });
        }
        else
        {
            if (_runeDict.ContainsKey(RuneType.Assist) == false)
            {

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

            if (changeIndex == -1)
            {
                Touch touch = Input.GetTouch(0);
                Vector3 pos = Camera.main.ScreenToWorldPoint(touch.position);
                UIManager.Instance.InfoMessagePopup("���� ���� ���� ������ �����ϴ�.", pos);
                return null;
            }

            if (!DummyCost.Instance.CanRune(card.Rune.AssistRune.Cost))
            {
                Touch touch = Input.GetTouch(0);
                Vector3 pos = Camera.main.ScreenToWorldPoint(touch.position);
                UIManager.Instance.InfoMessagePopup("������ �����մϴ�.", pos);
                return null;
            }

            // �ִϸ��̼� �ڷ� �̷��?
            Sequence seq = DOTween.Sequence();
            seq.AppendCallback(() =>
            {
                _isAddCard = true;
                //card.GetComponent<RectTransform>().anchoredPosition = Input.GetTouch(0).position;
                card.transform.SetParent(this.transform);
                //card.GetComponent<RectTransform>().anchoredPosition = card.GetComponent<RectTransform>().anchoredPosition;
                //card.SetCoolTime(card.Rune.AssistRune.DelayTurn);
                card.SetIsEquip(true);
                DummyCost.Instance.CanUseSubRune(card.Rune.AssistRune.Cost);
                if (_runeDict[RuneType.Assist][changeIndex] != null)
                {
                    card.GetComponent<RectTransform>().DOAnchorPos(_runeDict[RuneType.Assist][changeIndex].GetComponent<RectTransform>().anchoredPosition, 0.3f).OnComplete(() =>
                    {
                        card.SetOutlineActive(true);
                        Destroy(_runeDict[RuneType.Assist][changeIndex].gameObject); // Argument error ��¥ ���� ���±��� 1�� ��
                        _runeDict[RuneType.Assist][changeIndex] = card;
                        AddEffect(card, false);
                        //UpdateMagicName();
                        _effectContent.AddEffect(card.Rune, false);
                        _cardCollector.CardSort();
                        _isAddCard = false;
                    });
                }
            });
            seq.AppendCallback(() =>
            {
                SortCard();
                //UpdateMagicName();
                StartCoroutine(PlayEffect(card.Rune.RuneAudio));
                _cardCollector.CardSort();
            });
        }
        //SortCard();

        _cardCollector.UpdateCardOutline();
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

                    _effectDict[e.EffectType].Add(new EffectObjectPair(e, card.Rune.RuneEffect));
                }
                else
                {
                    _effectDict.Add(e.EffectType, new List<EffectObjectPair> { new EffectObjectPair(e, card.Rune.RuneEffect) });
                }
            }
        }
        else
        {
            foreach (Pair e in card.Rune.AssistRune.EffectDescription)
            {
                if (_effectDict.ContainsKey(e.EffectType))
                {

                    _effectDict[e.EffectType].Add(new EffectObjectPair(e, card.Rune.RuneEffect));
                }
                else
                {
                    _effectDict.Add(e.EffectType, new List<EffectObjectPair> { new EffectObjectPair(e, card.Rune.RuneEffect) });
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
                    name += $"�� {_runeDict[RuneType.Main][0].Rune.MainRune.Name}";
                }
                _nameText.text = name;
                #endregion
            }
        }
    }

    public void AssistRuneAnimanation()
    {
        if (_runeDict.ContainsKey(RuneType.Main) == false) return;

        SortCard();
        Sequence seq = DOTween.Sequence();
        //seq.AppendCallback(() => SortCard());
        seq.AppendCallback(() =>
        {
            if (_runeDict.ContainsKey(RuneType.Assist) == false)
            {
                for (int i = 0; i < _runeDict[RuneType.Main][0].Rune.MainRune.Cost; i++)
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

    bool isSelectCard = false;
    public void Swipe1()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchBeganPos = touch.position;
                isSelectCard = false;
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

                //��������. ��ġ�� x�̵��Ÿ��� y�̵��Ÿ��� �ΰ������� ũ��
                if (Mathf.Abs(touchDif.y) > swipeSensitivity || Mathf.Abs(touchDif.x) > swipeSensitivity)
                {
                    if (touchDif.y > 0 && Mathf.Abs(touchDif.y) > Mathf.Abs(touchDif.x))
                    {
                        //Debug.Log("up");
                    }
                    else if (touchDif.y < 0 && Mathf.Abs(touchDif.y) > Mathf.Abs(touchDif.x))
                    {
                        //Debug.Log("down");
                        if (_cardCollector.SelectCard == null && isSelectCard == false)
                        {
                            _cardCollector.CardRotate();
                        }
                    }
                    else if (touchDif.x > 0 && Mathf.Abs(touchDif.y) < Mathf.Abs(touchDif.x))
                    {
                        //Debug.Log("right");
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
                        //Debug.Log("Left");
                    }
                }
                //��ġ.
                else
                {
                    //Debug.Log("touch");
                }
            }
        }
    }


    public void Damage() // ���� ����
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
        seq.InsertCallback(0f, () =>
        {
            for (int i = _runeDict[RuneType.Assist].Count - 1; i >= 0; i--)
            {
                if (_runeDict[RuneType.Assist][i].Rune == null)
                {
                    Destroy(_runeDict[RuneType.Assist][i].gameObject);
                    _runeTempDict = _runeDict;
                    _runeDict[RuneType.Assist].RemoveAt(i);
                }
            }
            _runeTempDict = new Dictionary<RuneType, List<Card>>(_runeDict);
            _tempEffectDict = new Dictionary<EffectType, List<EffectObjectPair>>(_effectDict);
            _effectContent.AttackAnimation();
        });
        SoundManager.Instance.PlaySound(attackSound, SoundType.Effect);

        //int damage = 0;
        //seq.AppendInterval(0.1f);
        seq.AppendCallback(() =>
        {
            // �ο�, ���, ����, ���� ����

            //_effectContent.AttackAnimation();

            //AttackFunction(EffectType.Status);
            //AttackFunction(EffectType.Defence);
            //AttackFunction(EffectType.Attack);
            //AttackFunction(EffectType.Draw);
            //AttackFunction(EffectType.Destroy);

        });
        seq.AppendCallback(() =>
        {
            Debug.Log("Attack Complate");
            IsBig = false;

            if (_cardCollector.IsFront == false)
            {
                _cardCollector.CardRotate();
            }
            _cardCollector.IsFront = true;
            _cardCollector.UpdateCardOutline();

            foreach (var item in _runeDict)
            {
                foreach (Card card in item.Value)
                {
                    if (card.IsFront == false)
                    {
                        card.IsFront = true;
                    }
                    card.IsRest = true;
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

            _nameText.text = "";
            _effectText.text = "";
            _effectContent.Clear();
            _runeDict.Clear();
            _effectDict.Clear();

            _cardCollector.UpdateCardOutline();
        });

        //enemy.Damage(damage);
    }

    public void AttackFunction(EffectType effectType)
    {
        if (_effectDict.ContainsKey(effectType))
        {
            foreach (var e in _effectDict[effectType])
            {
                Unit target = e.pair.IsEnemy == true ? GameManager.Instance.enemy : GameManager.Instance.player;
                AttackEffectFunction(effectType, target, e.pair)?.Invoke();
            }
        }
    }

    public Action AttackEffectFunction(EffectType effectType, Unit target, Pair e)
    {
        Action action = null;
        int c = 0;
        for (int i = 0; i < _runeTempDict[RuneType.Assist].Count; i++)
        {
            if (_runeTempDict[RuneType.Assist][i].Rune != null && _runeTempDict[RuneType.Assist][i].Rune.AssistRune.Attribute == e.AttributeType)
            {
                c++;
            }
        }
        if (_runeTempDict[RuneType.Main][0].Rune != null && _runeTempDict[RuneType.Main][0].Rune.AssistRune.Attribute == e.AttributeType)
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
                action = () => _cardCollector.CardDraw(int.Parse(e.Effect));
                break;
            case EffectType.Etc:
                break;
        }

        switch (e.Condition.ConditionType)
        {
            case ConditionType.None:
                return action;
            case ConditionType.HeathComparison:
                if (target.IsHealthAmount(e.Condition.Value, e.Condition.ComparisonType))
                {
                    return action;
                }
                break;
            case ConditionType.AssistRuneCount:
                int count = 0;
                for (int i = 0; i < _runeTempDict[RuneType.Assist].Count; i++)
                {
                    if (_runeTempDict[RuneType.Assist][i].Rune != null)
                    {
                        count++;
                    }
                }
                if (count >= e.Condition.Value)
                {
                    return action;
                }
                break;
            case ConditionType.AttributeComparison:
                int cnt = 0;
                if (_runeTempDict[RuneType.Main][0].Rune.MainRune.Attribute == e.Condition.AttributeType)
                    cnt++;
                for (int i = 0; i < _runeTempDict[RuneType.Assist].Count; i++)
                {
                    if (_runeTempDict[RuneType.Assist][i].Rune.AssistRune.Attribute == e.Condition.AttributeType)
                    {
                        cnt++;
                    }
                }

                switch (e.Condition.ComparisonType)
                {
                    case ComparisonType.MoreThan:
                        if (cnt >= e.Condition.Value)
                        {
                            return action;
                        }
                        break;
                    case ComparisonType.LessThan:
                        if (cnt <= e.Condition.Value)
                        {
                            return action;
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
                            return action;
                        }
                        break;
                    case ComparisonType.LessThan:
                        if (StatusManager.Instance.GetUnitStatusValue(target, e.Condition.StatusType) <= e.Condition.Value)
                        {
                            return action;
                        }
                        break;
                }
                break;
        }

        return null;
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
