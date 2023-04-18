using DG.Tweening;
using MoreMountains.Tools;
using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
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
    private List<BaseRuneUI> _remainingRuneList = new List<BaseRuneUI>(20);

    private Transform _remamingRuneContainer;
    #endregion

    private bool _isAttack;

    private DialScene _dialScene;

    private void Awake()
    {
        _remamingRuneContainer = transform.Find("RuneContainer");

        _runeDict = new Dictionary<int, List<BaseRuneUI>>(3);
        for (int i = 1; i <= 3; i++)
        {
            _runeDict.Add(i, new List<BaseRuneUI>());
        }
        _dialElementList = new List<DialElement>();

        for (int i = 0; i < Managers.Deck.Deck.Count; i++)
        {
            BaseRuneUI r = Managers.Resource.Instantiate("Rune/BaseRune", _remamingRuneContainer).GetComponent<BaseRuneUI>();
            r.SetRune(Managers.Deck.Deck[i]);
            //r.Rune.Init();
            r.gameObject.SetActive(false);
            _remainingRuneList.Add(r);
        }

        _isAttack = false;
    }

    private void Start()
    {
        _dialScene = Managers.Scene.CurrentScene as DialScene;

        for (int i = 0; i < 3; i++)
        {
            DialElement d = this.transform.GetChild(i).GetComponent<DialElement>();
            d.SetLineID(3 - i);

            _dialElementList.Add(d);
        }

        SettingDialRune(true);
    }

    public void SortingRemaingRune()
    {
        _remainingRuneList.OrderBy(z => z.Rune.CoolTIme);
    }

    public int GetUsingRuneCount()
    {
        int count = 0;
        for (int i = 0; i < Managers.Deck.Deck.Count; i++)
        {
            if (Managers.Deck.Deck[i].CoolTIme <= 0)
            {
                count++;
            }
        }

        return count;
    }

    public void SettingDialRune(bool isReset)
    {
        #region Clear
        foreach (var runeList in _runeDict)
        {
            for (int i = 0; i < runeList.Value.Count; i++)
            {
                runeList.Value[i].gameObject.SetActive(false);
                runeList.Value[i].transform.SetParent(_remamingRuneContainer);
                _remainingRuneList.Add(runeList.Value[i]);
            }
        }
        _runeDict.Clear();
        for (int i = 0; i < _dialElementList.Count; i++)
        {
            _dialElementList[i].ResetRuneList();
            _dialElementList[i].SelectCard = null;
        }
        #endregion

        SortingRemaingRune();
        int maxRuneCount = GetUsingRuneCount();

        List<int> numberList = new List<int>();
        for (int i = 0; i < Managers.Deck.FirstDialDeck.Count; i++)
        {
            numberList.Add(i);
        }

        if (Managers.Deck.FirstDialDeck != null && Managers.Deck.FirstDialDeck.Count > 0)
        {
            for (int i = 0; i < _maxRuneCount; i++)
            {
                int randomIndex = Random.Range(0, numberList.Count);
                BaseRuneUI r = _remainingRuneList.Find(x => x.Rune == Managers.Deck.FirstDialDeck[randomIndex]);
                r.transform.SetParent(_dialElementList[2].transform);
                r.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
                r.gameObject.SetActive(true);
                _dialElementList[2].AddRuneList(r);
                if (isReset == true)
                {
                    r.Rune.SetCoolTime(0);
                }
                AddCard(r, 1);

                numberList.RemoveAt(randomIndex);
                _remainingRuneList.Remove(r);
                maxRuneCount--;
            }
        }
        else
        {
            SortingRemaingRune();
            maxRuneCount = GetUsingRuneCount();
            for (int i = 0; i < _maxRuneCount; i++)
            {
                if (maxRuneCount <= 0)
                {
                    break;
                }

                int runeIndex = Random.Range(0, maxRuneCount);

                BaseRuneUI r = _remainingRuneList[runeIndex];
                r.transform.SetParent(_dialElementList[2].transform);
                r.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
                r.gameObject.SetActive(true);
                _dialElementList[2].AddRuneList(r);
                if (isReset == true)
                {
                    r.Rune.SetCoolTime(0);
                }
                AddCard(r, 1);
                Managers.Deck.RuneSwap(runeIndex, maxRuneCount - 1);

                _remainingRuneList.RemoveAt(runeIndex);
                maxRuneCount--;
            }
        }

        for (int i = 0; i < _maxRuneCount * 2; i++)
        {
            if (maxRuneCount <= 0)
            {
                break;
            }

            int runeIndex = Random.Range(0, maxRuneCount);

            int index = 1 - (i % 2);
            while (Managers.Deck.FirstDialDeck.Find(x => x == _remainingRuneList[runeIndex].Rune) != null)
            {
                InfiniteLoopDetector.Run();
                runeIndex = Random.Range(0, maxRuneCount);
            }
            BaseRuneUI r = _remainingRuneList[runeIndex];
            r.transform.SetParent(_dialElementList[index].transform);
            r.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
            r.gameObject.SetActive(true);
            _dialElementList[index].AddRuneList(r);
            if (isReset == true)
            {
                r.Rune.SetCoolTime(0);
            }
            AddCard(r, 3 - index);
            Managers.Deck.RuneSwap(runeIndex, maxRuneCount - 1);

            _remainingRuneList.RemoveAt(runeIndex);

            maxRuneCount--;
        }

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
                        r.transform.localScale = new Vector3(0.1f, 0.1f);
                        _dialElementList[3 - i].AddRuneList(r);

                        if (isReset == true)
                        {
                            r.Rune.SetCoolTime(0);
                        }
                        AddCard(r, i);
                    }
                }
            }
        }

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
                //_magicDict[1][i].transform.localScale = new Vector3(0.02f, 0.02f, 1);

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

    public void Attack()
    {
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
        for (int i = _dialElementList.Count - 1; i >= 0; i--)
        {
            if (_dialElementList[i].SelectCard != null)
            {
                int index = i;
                _dialElementList[i].IsGlow = true;
                BezierMissile b = Managers.Resource.Instantiate("BezierMissile", this.transform.parent).GetComponent<BezierMissile>();
                if (_dialElementList[i].SelectCard.Rune.BaseRuneSO.RuneEffect != null)
                {
                    b.SetEffect(_dialElementList[i].SelectCard.Rune.BaseRuneSO.RuneEffect);
                }
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
        for (int i = 0; i < Managers.Deck.Deck.Count; i++)
        {
            if (Managers.Deck.Deck[i].CoolTIme > 0)
            {
                Managers.Deck.Deck[i].AddCoolTime(-1);
            }
        }
    }
}