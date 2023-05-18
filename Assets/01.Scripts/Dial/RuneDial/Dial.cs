using DG.Tweening;
using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;
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

    #region DialElement Drag
    private DialElement _selectDialElement;
    private int _selectIndex = -1;

    private bool _isDialSelect = false;
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

    public void SelectDialElement(DialElement e)
    {
        if (_selectDialElement == e) return;

        int fingerId = -1;
        if (_selectDialElement != null)
        {
            _selectDialElement.DialState = DialState.None;
            fingerId = _selectDialElement.FingerID;
            _selectDialElement.IsTouchDown = false;
            _selectDialElement.IsGlow = false;
        }
        _selectDialElement = e;
        if(_selectDialElement != null)
        {
            _selectDialElement.DialState = DialState.Drag;
            if(fingerId != -1)
            {
                _selectDialElement.FingerID = fingerId;
            }
            _selectDialElement.IsTouchDown = true;
            _selectDialElement.IsGlow = true;
        }

        if(e == null)
        {
            _selectIndex = -1;
        }
        else
        {
            for(int i = 0; i < _dialElementList.Count; i++)
            {
                if (_dialElementList[i] == e)
                {
                    _selectIndex = i;
                    break;
                }
            }
        }

        _isDialSelect = _selectDialElement != null;
    }

    public void SelectDialElement(in int index)
    {
        if(_selectIndex == index) return;

        //SelectDialElement(_dialElementList[index]);

        int fingerId = -1;
        if (_selectDialElement != null)
        {
            _selectDialElement.DialState = DialState.None;
            _selectDialElement.IsTouchDown = false;
            fingerId = _selectDialElement.FingerID;
            _selectDialElement.IsGlow = false;
        }

        if (index == -1)
        {
            _selectDialElement = null;
        }
        else
        {
            _selectDialElement = _dialElementList[index];
            _selectDialElement.DialState = DialState.Drag;
            _selectDialElement.IsTouchDown = true;
            _selectDialElement.IsGlow = true;
            if (fingerId != -1)
            {
                _selectDialElement.FingerID = fingerId;
            }
        }

        if (_selectDialElement != null)
        {
            _selectDialElement.DialState = DialState.Drag;
        }

        _selectIndex = index;

        _isDialSelect = _selectDialElement != null;
    }

    private void Update()
    {
        if(_isDialSelect == true && _selectIndex != -1)
        {
            Debug.Log("Select Dial");
            if (_selectDialElement == null || _selectDialElement.FingerID == -1) return;
            Touch touch = Input.GetTouch(_selectDialElement.FingerID);

            switch (touch.phase)
            {
                case TouchPhase.Moved:
                    float distance = Mathf.Abs(Vector2.Distance(transform.position, Define.MainCam.ScreenToWorldPoint(touch.position)));

                    if (_dialElementList[2].InDistance <= distance)
                    {
                        for (int i = _dialElementList.Count - 1; i >= 0; i--)
                        {
                            if (_dialElementList[i].OutDistance >= distance)
                            {
                                int fIndex = 3 - _selectIndex;
                                int sIndex = 3 - i;
                                LineSwap(fIndex, sIndex);
                                SelectDialElement(i);
                                break;
                            }
                        }
                    }
                    break;
                case TouchPhase.Ended:
                    SelectDialElement(null);
                    _isDialSelect = false;
                    break;
            }
        }
    }

    public void LineSwap(int fLine, int sLine)
    {
        if (fLine == sLine) return;

        if(_runeDict.ContainsKey(fLine) == false || _runeDict.ContainsKey(sLine) == false)
        {
            Debug.LogWarning("Not have Key");
            return;
        }

        List<BaseRuneUI> newList = new List<BaseRuneUI>(_runeDict[fLine]);
        _runeDict[fLine].Clear();
        _runeDict[fLine] = new List<BaseRuneUI>(_runeDict[sLine]);
        _runeDict[sLine].Clear();
        _runeDict[sLine] = new List<BaseRuneUI>(newList);

        int dialFLine = 3 - fLine;
        int dialSLine = 3 - sLine;
        _dialElementList[dialFLine].SetRuneList(_runeDict[fLine]);
        _dialElementList[dialSLine].SetRuneList(_runeDict[sLine]);

        float offset = _lineDistanceArray[dialFLine] / _lineDistanceArray[dialSLine];
        for (int i = 0; i < _dialElementList[dialFLine].RuneList.Count; i++)
        {
            // y값이 이상해!
            //_dialElementList[dialFLine].RuneList[i].transform.DOKill();
            //_dialElementList[dialFLine].RuneList[i].transform.DOMove(_dialElementList[dialFLine].RuneList[i].transform.position * offset/* + this.transform.position*/, 0.2f);
            _dialElementList[dialFLine].RuneList[i].transform.localScale = new Vector3(0.1f, 0.1f, 1f);
            _dialElementList[dialFLine].RuneList[i].transform.SetParent(_dialElementList[dialFLine].transform);
        }

        offset = _lineDistanceArray[dialSLine] / _lineDistanceArray[dialFLine];
        for (int i = 0; i < _dialElementList[dialSLine].RuneList.Count; i++)
        {
            //_dialElementList[dialSLine].RuneList[i].transform.DOKill();
            //_dialElementList[dialSLine].RuneList[i].transform.DOMove(_dialElementList[dialSLine].RuneList[i].transform.position * offset/* + this.transform.position*/, 0.2f);
            _dialElementList[dialSLine].RuneList[i].transform.localScale = new Vector3(0.1f, 0.1f, 1f);
            _dialElementList[dialSLine].RuneList[i].transform.SetParent(_dialElementList[dialSLine].transform);
        }

        //float zRotate = _dialElementList[dialFLine].transform.rotation.z;
        //_dialElementList[dialFLine].transform.rotation = Quaternion.Euler(0, 0, _dialElementList[dialSLine].transform.rotation.z);
        //_dialElementList[dialSLine].transform.rotation = Quaternion.Euler(0, 0, zRotate);
        //int num = 0 + 1 + 2;
        //num -= dialFLine;
        //num -= dialSLine;
        //_dialElementList[num].transform.rotation = Quaternion.Euler(0, 0, 0);

        for (int i = 0; i < _dialElementList.Count; i++)
        {
            _dialElementList[i].transform.rotation = Quaternion.Euler(Vector3.zero);
        }

        RuneSort(true);
        //for (int i = 1; i <= 3; i++)
        //{
        //    RuneMove(i, true);
        //}
    }

    public void RuneMove(int line, bool isTween = false)
    {
        if (_runeDict.ContainsKey(line))
        {
            for (int i = 0; i < _runeDict[line].Count; i++)
            {
                float fAngle = Mathf.Atan2(_runeDict[line][i].transform.position.y - this.transform.position.y, _runeDict[line][i].transform.position.x - this.transform.position.x)/* * Mathf.Deg2Rad *//*180 / Mathf.PI*/;
                //if (this.transform.position.x > _runeDict[line][i].transform.position.y)
                //{
                //    fAngle = 360 - fAngle;
                //    // 0~180 도는 구할 수 있지만, y1 > y2 인 경우.. 즉
                //    // 3사분면 4사분면은 360 - (180~360) 를 해줘서 [0,180]
                //    // 값으로 보정해준다.
                //}                                                                                                                                                                                           

                float height = Mathf.Sin((fAngle + 90) * Mathf.Deg2Rad) * _lineDistanceArray[3 - line];
                float width = Mathf.Cos((fAngle + 90) * Mathf.Deg2Rad) * _lineDistanceArray[3 - line];
                if (isTween)
                {
                    transform.DOKill();
                    _runeDict[line][i].transform.DOMove(new Vector3(width + this.transform.position.x, height + this.transform.position.y, 0), 0.2f);
                }
                else
                {
                    _runeDict[line][i].transform.position = new Vector3(width + this.transform.position.x, height + this.transform.position.y, 0);
                }

                Vector2 direction = new Vector2(
                    (width + this.transform.position.x) - transform.position.x,
                    (height + this.transform.position.y) - transform.position.y
                );

                float ang = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion angleAxis = Quaternion.AngleAxis(ang - 90f, Vector3.forward);
                _runeDict[line][i].transform.rotation = angleAxis;
            }
        }
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
    
        for (int i = 0; i < _maxRuneCount * 3; i++)
        {
            if (_remainingDeck.Count <= 0)
            {
                break;
            }

            int runeIndex = Random.Range(0, _remainingDeck.Count);

            int index = 2 - (i % 3);
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
        Define.DialScene.CardDescPopup(DialElementList[2].SelectCard.Rune);
    }

    public void AddCard(in BaseRuneUI card, in int tier)
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

    private void RuneSort(bool isTween = false)
    {
        for (int i = 1; i <= 3; i++)
        {
            LineCardSort(i, isTween);
        }
    }

    public void LineCardSort(int line, bool isTween = false, float tweenDuration = 0.2f)
    {
        if (_runeDict.ContainsKey(line))
        {
            float angle = -1 * _runeAngle / _runeDict[line].Count * Mathf.Deg2Rad;

            for (int i = 0; i < _runeDict[line].Count; i++)
            {
                float radianValue = angle * i + (_startAngle * Mathf.Deg2Rad);

                float height = Mathf.Sin(radianValue) * _lineDistanceArray[3 - line];
                float width = Mathf.Cos(radianValue) * _lineDistanceArray[3 - line];
                Transform tr = _runeDict[line][i].transform;
                //if (_dialElementList[3 - line].SelectCard != null)
                //{
                //    tr = RuneTransformBySelectRune(line, i);
                //}
                //else
                //{
                //    tr = _runeDict[line][i].transform;
                //}
                if (isTween)
                {
                    tr.DOKill();
                    tr.DOMove(new Vector3(width + this.transform.position.x, height + this.transform.position.y, 0), tweenDuration);
                }
                else
                {
                    tr.position = new Vector3(width + this.transform.position.x, height + this.transform.position.y, 0);
                }

                Vector2 direction = new Vector2(
                    (width + this.transform.position.x) - transform.position.x,
                    (height + this.transform.position.y) - transform.position.y
                );

                float ang = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion angleAxis = Quaternion.AngleAxis(ang - 90f, Vector3.forward);
                _runeDict[line][i].transform.rotation = angleAxis;
            }
        }
    }

    public Transform RuneTransformBySelectRune(int line, int index)
    {
        List<BaseRuneUI> newList = new List<BaseRuneUI>(_runeDict[line]);

        int count = newList.Count;
        for (int i = 0; i < count; i++)
        {
            if (newList[0] == _dialElementList[3 - line].SelectCard)
                break;
            else
            {
                BaseRuneUI rune = newList[0];
                newList.RemoveAt(0);
                newList.Add(rune);
            }
        }

        return newList[index].transform;
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

    public bool MagicEmpty(bool isAll = true)
    {
        if (isAll)
        {
            return _dialElementList[0].SelectCard == null && _dialElementList[1].SelectCard == null && _dialElementList[2].SelectCard == null;
        }
        else
        {
            return _dialElementList[0].SelectCard == null || _dialElementList[1].SelectCard == null || _dialElementList[2].SelectCard == null;
        }
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
        //Define.DialScene?.CardDescDown();

        // 이 부분 예외처리 필요
        AttributeType compareAttributeType = _dialElementList[0].SelectCard.Rune.BaseRuneSO.AttributeType;
        bool isResonanceCheck = true;
        for (int i = _dialElementList.Count - 1; i >= 0; i--)
        {
            if (_dialElementList[i].SelectCard != null)
            {
                int index = i;
                MagicCircleGlow(i, true);
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

                Transform pos = null;
                float bendValue = 0f;
                switch (_dialElementList[i].SelectCard.Rune.BaseRuneSO.Direction)
                {
                    case EffectDirection.Enemy:
                        pos = BattleManager.Instance.Enemy.transform;
                        bendValue = 7f;
                        break;
                    case EffectDirection.Player:
                        pos = this.transform;
                        bendValue = 15f;
                        break;
                }
                Managers.Sound.PlaySound(_dialElementList[i].SelectCard.Rune.BaseRuneSO.RuneSound, SoundType.Effect);
                b.Init(_dialElementList[i].SelectCard.transform, pos, 1.5f, bendValue, bendValue, () =>
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

    public void MagicCircleGlow(int index, bool value)
    {
        _dialElementList[index].IsGlow = value;
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
        if (MagicEmpty(false))
        {
            _resonance.ActiveAllEffectObject(false);
        }
        else
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
                _resonance.ActiveAllEffectObject(false);
        }
    }
}
