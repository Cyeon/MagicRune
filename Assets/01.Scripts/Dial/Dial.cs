using DG.Tweening;
using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;
using Random = UnityEngine.Random;

public class Dial : MonoBehaviour
{
    #region Rotate Parameta
    [SerializeField]
    private int _maxRuneCount = 3;
    [SerializeField, Range(90f, 360f)]
    private float _runeAngle = 180f;
    public float RuneAngle => _runeAngle;
    [SerializeField, Range(0f, 360f)]
    private float _startAngle = 180;
    public float StartAngle => _startAngle;
    #endregion

    #region Rune Parameta
    [SerializeField]
    private int _copyCount = 2;

    [SerializeField]
    private float[] _lineDistanceArray = new float[3];
    public float[] LineDistanceArray => _lineDistanceArray;
    #endregion

    #region Rune Container
    private Dictionary<int, List<BaseRuneUI>> _runeDict;
    private List<DialElement> _dialElementList;
    public List<DialElement> DialElementList => _dialElementList;

    private List<BaseRune> _remainingDeck = new List<BaseRune>();
    private List<BaseRune> _usingDeck = new List<BaseRune>();
    private List<BaseRune> _cooltimeDeck = new List<BaseRune>();

    #endregion

    private bool _isAttack;
    private Resonance _resonance;

    private void Awake()
    {
        #region Initialization 
        _resonance = GetComponent<Resonance>();

        _runeDict = new Dictionary<int, List<BaseRuneUI>>(3);
        for (int i = 1; i <= 3; i++)
        {
            _runeDict.Add(i, new List<BaseRuneUI>());
        }
        _dialElementList = new List<DialElement>();
        #endregion

        _isAttack = false;
    }

    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            DialElement d = this.transform.GetChild(i).GetComponent<DialElement>();
            d.SetLineID(3 - i);

            _dialElementList.Add(d);
        }

        SettingDialRune(true);
    }

    public void SettingDialRune(bool isReset)
    {
        #region Clear
        foreach (var runeList in _runeDict)
        {
            for (int i = 0; i < runeList.Value.Count; i++)
            {
                Managers.Resource.Destroy(runeList.Value[i].gameObject);
            }
        }
        _runeDict.Clear();

        for (int i = _usingDeck.Count - 1; i >= 0; i--)
        {
            _remainingDeck.Add(_usingDeck[i]);
            _usingDeck.RemoveAt(i);
        }
        _usingDeck.Clear();

        for (int i = _cooltimeDeck.Count - 1; i >= 0; i--)
        {
            if (_cooltimeDeck[i].IsCoolTime == false)
            {
                _remainingDeck.Add(_cooltimeDeck[i]);
                _cooltimeDeck.RemoveAt(i);
            }
        }

        for (int i = 0; i < _dialElementList.Count; i++)
        {
            _dialElementList[i].ResetRuneList();
            _dialElementList[i].SelectCard = null;
        }

        if (isReset == true)
        {
            for (int i = 0; i < Managers.Deck.Deck.Count; i++)
            {
                Managers.Deck.Deck[i].SetCoolTime(0);
            }
            _remainingDeck.Clear();
            _remainingDeck = new List<BaseRune>(Managers.Deck.Deck);
            _cooltimeDeck.Clear();
        }
        #endregion

        #region First Line
        for (int i = 0; i < Managers.Deck.FirstDialDeck.Count; i++)
        {
            BaseRuneUI r = Managers.Resource.Instantiate("Rune/BaseRune").GetComponent<BaseRuneUI>();
            BaseRune rune = _remainingDeck.Find(x => x.BaseRuneSO == Managers.Deck.FirstDialDeck[i].BaseRuneSO && x.CoolTime <= 0);
            if (rune == null)
            {
                int runeIndex = Random.Range(0, _remainingDeck.Count);

                rune = _remainingDeck[runeIndex];
            }
            r.gameObject.SetActive(true);
            _dialElementList[2].AddRuneList(r);
            AddCard(r, 1);
            r.transform.SetParent(_dialElementList[2].transform);
            r.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
            r.SetRune(rune);
            _remainingDeck.Remove(rune);
            _usingDeck.Add(rune);
        }

        for (int i = 0; i < _maxRuneCount - Managers.Deck.FirstDialDeck.Count; i++)
        {
            int runeIndex = Random.Range(0, _remainingDeck.Count);

            BaseRuneUI r = Managers.Resource.Instantiate("Rune/BaseRune").GetComponent<BaseRuneUI>();
            BaseRune rune = _remainingDeck[runeIndex];
            r.SetRune(rune);
            r.transform.SetParent(_dialElementList[2].transform);
            r.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
            r.gameObject.SetActive(true);
            _dialElementList[2].AddRuneList(r);
            AddCard(r, 1);
            _remainingDeck.Remove(rune);
            _usingDeck.Add(rune);
        }
        #endregion

        #region Second&Third Line
        for (int i = 0; i < _maxRuneCount * 2; i++)
        {
            if (_remainingDeck.Count <= 0)
            {
                break;
            }

            int runeIndex = Random.Range(0, _remainingDeck.Count);

            int index = 1 - (i % 2);
            BaseRune rune = _remainingDeck[runeIndex];
            BaseRuneUI r = Managers.Resource.Instantiate("Rune/BaseRune").GetComponent<BaseRuneUI>();
            r.SetRune(rune);
            r.transform.SetParent(_dialElementList[index].transform);
            r.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
            r.gameObject.SetActive(true);
            _dialElementList[index].AddRuneList(r);
            AddCard(r, 3 - index);
            _remainingDeck.Remove(rune);
            _usingDeck.Add(rune);
        }
        #endregion

        #region Copy
        for (int i = 1; i <= 3; i++)
        {
            if (_runeDict.ContainsKey(i))
            {
                int count = _runeDict[i].Count;
                for (int k = 0; k < _copyCount; k++)
                {
                    for (int j = 0; j < count; j++)
                    {
                        BaseRuneUI r = Managers.Resource.Instantiate("Rune/BaseRune", _dialElementList[3 - i].transform).GetComponent<BaseRuneUI>();
                        r.SetRune(_runeDict[i][j].Rune);
                        r.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
                        _dialElementList[3 - i].AddRuneList(r);

                        AddCard(r, i);
                    }
                }
            }
        }
        #endregion

        RuneSort();
    }

    public void AddCard(BaseRuneUI card, int tier)
    {
        if (card != null)
        {
            if (_runeDict.ContainsKey(tier))
            {
                _runeDict[tier].Add(card);
            }
            else
            {
                _runeDict.Add(tier, new List<BaseRuneUI> { card });
            }

            RuneSort();
        }
    }

    private void RuneSort()
    {
        for (int i = 1; i <= 3; i++)
        {
            LineCardSort(i);
        }
    }

    public void LineCardSort(int line)
    {
        if (_runeDict.ContainsKey(line))
        {
            float angle = -1 * _runeAngle / _runeDict[line].Count * Mathf.Deg2Rad;

            for (int i = 0; i < _runeDict[line].Count; i++)
            {
                float radianValue = angle * i + (_startAngle * Mathf.Deg2Rad);

                float height = Mathf.Sin(radianValue) * _lineDistanceArray[3 - line];
                float width = Mathf.Cos(radianValue) * _lineDistanceArray[3 - line];
                _runeDict[line][i].transform.position = new Vector3(width + this.transform.position.x, height + this.transform.position.y, 0);

                Vector2 direction = new Vector2(
                    _runeDict[line][i].transform.position.x - transform.position.x,
                    _runeDict[line][i].transform.position.y - transform.position.y
                );

                float ang = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion angleAxis = Quaternion.AngleAxis(ang - 90f, Vector3.forward);
                _runeDict[line][i].transform.rotation = angleAxis;
            }
        }
    }

    public void ResetDial()
    {
        for (int i = 0; i < _dialElementList.Count; i++)
        {
            _dialElementList[i].transform.eulerAngles = Vector3.zero;
        }
    }

    public void AllMagicActive(bool value)
    {
        for (int i = 1; i <= 3; i++)
        {
            if (_runeDict.ContainsKey(i))
            {
                for (int j = 0; j < _runeDict[i].Count; j++)
                {
                    _runeDict[3][j].gameObject.SetActive(false);
                }
            }
        }
    }

    public bool MagicEmpty()
    {
        return _dialElementList[0].SelectCard == null && _dialElementList[1].SelectCard == null && _dialElementList[2].SelectCard == null;
    }

    public void Attack()
    {
        if (MagicEmpty() == true) return;

        if (_isAttack == true) return;
        if (BattleManager.Instance.GameTurn == GameTurn.Player)
        {
            _isAttack = true;

            StartCoroutine(AttackCoroutine());
            _isAttack = false;
        }
    }

    private IEnumerator AttackCoroutine()
    {
        Define.DialScene?.CardDescDown();

        // 이 부분 예외처리 필요
        AttributeType compareAttributeType = _dialElementList[0].SelectCard.Rune.BaseRuneSO.AttributeType;
        bool isResonanceCheck = true;
        for (int i = _dialElementList.Count - 1; i >= 0; i--)
        {
            if (_dialElementList[i].SelectCard != null)
            {
                int index = i;
                _dialElementList[i].IsGlow = true;
                BaseRune rune = _usingDeck.Find(x => x == _dialElementList[i].SelectCard.Rune);
                _usingDeck.Remove(rune);
                rune.SetCoolTime();
                _cooltimeDeck.Add(rune);
                BezierMissile b = Managers.Resource.Instantiate("BezierMissile", this.transform.parent).GetComponent<BezierMissile>();
                if (_dialElementList[i].SelectCard.Rune.BaseRuneSO.RuneEffect != null)
                {
                    b.SetEffect(_dialElementList[i].SelectCard.Rune.BaseRuneSO.RuneEffect);
                }

                if (isResonanceCheck)
                    isResonanceCheck = _dialElementList[i].SelectCard.Rune.BaseRuneSO.AttributeType == compareAttributeType;

                switch (_dialElementList[i].SelectCard.Rune.BaseRuneSO.AttributeType)
                {
                    case AttributeType.None:
                        break;
                    case AttributeType.NonAttribute:
                        b.SetTrailColor(Color.gray);
                        break;
                    case AttributeType.Fire:
                        b.SetTrailColor(Color.red);
                        break;
                    case AttributeType.Ice:
                        b.SetTrailColor(Color.cyan);
                        break;
                    case AttributeType.Wind:
                        b.SetTrailColor(new Color(0, 1, 0));
                        break;
                    case AttributeType.Ground:
                        b.SetTrailColor(new Color(0.53f, 0.27f, 0));
                        break;
                    case AttributeType.Electric:
                        b.SetTrailColor(Color.yellow);
                        break;
                }

                b.Init(_dialElementList[i].SelectCard.transform, BattleManager.Instance.Enemy.transform, 1.5f, 7, 7, () =>
                {
                    _dialElementList[index].Attack();
                    //_dialElementList[i] = null;
                });

                BattleManager.Instance.missileCount += 1;
                yield return new WaitForSeconds(0.1f);
            }
        }

        if (isResonanceCheck)
        {
            _resonance.Invocation(compareAttributeType);
        }
    }

    public void AllMagicCircleGlow(bool value)
    {
        for (int i = 0; i < _dialElementList.Count; i++)
        {
            _dialElementList[i].IsGlow = value;
        }
    }

    public void AllMagicSetCoolTime()
    {
        Debug.Log("쿨타임 감소");

        for (int i = 0; i < _cooltimeDeck.Count; i++)
        {
            if (_cooltimeDeck[i].IsCoolTime == true)
            {
                _cooltimeDeck[i].AddCoolTime(-1);
            }
        }
    }

    public void CheckResonance()
    {
        AttributeType criterionType = _dialElementList[0].SelectCard.Rune.BaseRuneSO.AttributeType;
        bool isSame = true;

        for (int i = 1; i < _dialElementList.Count; i++)
        {
            isSame = criterionType == _dialElementList[i].SelectCard.Rune.BaseRuneSO.AttributeType;
            if (!isSame)
                break;
        }

        if (isSame)
            _resonance.ResonanceEffect(criterionType);
        else
            _resonance.ResonanceEffect(AttributeType.None, false);
    }
}