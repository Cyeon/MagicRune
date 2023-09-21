using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T1">????????????????????ㅻ쑋???????????????????????????????????????嫄??????????????嫄????????????됰슣類????????????????????????ex) BaseRuneUI</typeparam>
/// <typeparam name="T2">T1???????????ex) BaseRune</typeparam>
public class Dial<T1, T2> : MonoBehaviour where T1 : MonoBehaviour where T2 : class
{
    #region Rotate Parameta
    [SerializeField]
    protected int _maxCount = 3;
    [SerializeField, Range(90f, 360f)]
    private float _dialAngle = 180f;
    public float DialAngle => _dialAngle;
    [SerializeField, Range(0f, 360f)]
    private float _startAngle = 180;
    public float StartAngle => _startAngle;
    #endregion

    #region Parameta
    [SerializeField]
    protected int _copyCount = 2;

    [SerializeField]
    private float[] _lineDistanceArray = new float[3];
    public float[] LineDistanceArray => _lineDistanceArray;
    #endregion

    #region Container
    protected Dictionary<int, List<T1>> _elementDict;
    protected List<DialElement<T1, T2>> _dialElementList;
    public List<DialElement<T1, T2>> DialElementList => _dialElementList;

    protected List<T2> _remainingDeck = new List<T2>();
    protected List<T2> _usingDeck = new List<T2>();
    protected List<T2> _cooltimeDeck = new List<T2>();
    #endregion

    #region Drag Paremeta
    protected DialElement<T1, T2> _selectDialElement;
    protected int _selectIndex = -1;

    protected bool _isDialSelect = false;
    #endregion

    protected bool _isAttack;
    public bool IsAttack => _isAttack;
    protected virtual bool _isAttackCondition { get; }

    private bool _attackable = true;

    public Action OnDialAttack = null;
    public Action OnLineChange = null;

    protected virtual void Awake()
    {
        #region Initialization 
        _elementDict = new Dictionary<int, List<T1>>(3);
        for (int i = 1; i <= 3; i++)
        {
            _elementDict.Add(i, new List<T1>());
        }
        _dialElementList = new List<DialElement<T1, T2>>();

        for (int i = 0; i < _lineDistanceArray.Length; i++)
        {
            DialElement<T1, T2> d = this.transform.GetChild(i).GetComponent<DialElement<T1, T2>>();
            d.SetLineID(3 - i);

            _dialElementList.Add(d);
        }
        #endregion

        _isAttack = false;
    }

    protected virtual void Start()
    {
        SetAttackable(true);
        SettingDialRune(true);
        ResetDial();

        //Debug.Log($"{Quaternion.Euler(new Vector3(0, 0, 60)).eulerAngles}, {Quaternion.Euler(new Vector3(0, 0, 300)).eulerAngles}");
        //Debug.Log($"{new Quaternion(0, 0, 0.5f, -1).eulerAngles}, {new Quaternion(0, 0, 0.5f, 1).eulerAngles}");
        //Debug.Log($"{Quaternion.Euler(new Vector3(0, 0, 300))}");
    }

    public void SelectDialElement(DialElement<T1, T2> e)
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
        if (_selectDialElement != null)
        {
            _selectDialElement.DialState = DialState.Drag;
            if (fingerId != -1)
            {
                _selectDialElement.FingerID = fingerId;
            }
            _selectDialElement.IsTouchDown = true;
            _selectDialElement.IsGlow = true;
        }

        if (e == null)
        {
            _selectIndex = -1;
        }
        else
        {
            for (int i = 0; i < _dialElementList.Count; i++)
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
        if (_selectIndex == index) return;

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

    protected virtual void Update()
    {
        if (_isDialSelect == true && _selectIndex != -1 && _selectDialElement.IsUsingLineSwap == true)
        {
            if (_selectDialElement == null || _selectDialElement.FingerID == -1) return;
            Touch touch = Input.GetTouch(_selectDialElement.FingerID);

            switch (touch.phase)
            {
                case TouchPhase.Moved:
                    float distance = Mathf.Abs(Vector2.Distance(transform.position, Define.MainCam.ScreenToWorldPoint(touch.position)));

                    // ????????????????????力?肉?????????????饔낅떽?????猷몄구????????????關?쒎첎?嫄??怨몃룯??????
                    if (_dialElementList[2].InDistance <= distance)
                    {
                        for (int i = _dialElementList.Count - 1; i >= 0; i--)
                        {
                            if (_dialElementList[i].OutDistance >= distance)
                            {
                                int fIndex = 3 - _selectIndex;
                                int sIndex = 3 - i;
                                StartCoroutine(LineSwap(fIndex, sIndex));
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

    public IEnumerator LineSwap(int fLine, int sLine)
    {
        if (fLine == sLine) yield break;

        if (_elementDict.ContainsKey(fLine) == false || _elementDict.ContainsKey(sLine) == false)
        {  
            //Debug.LogWarning("Not have Key");
            yield break;
        }

        int dialFLine = 3 - fLine;
        int dialSLine = 3 - sLine;

        _dialElementList[dialFLine].ElementMoveInLine(false);
        _dialElementList[dialSLine].ElementMoveInLine(false);
        for (int i = 0; i < _dialElementList[dialFLine].ElementList.Count; i++)
        {
            _dialElementList[dialFLine].ElementList[i].transform.DOComplete(true);
        }
        for (int i = 0; i < _dialElementList[dialSLine].ElementList.Count; i++)
        {
            _dialElementList[dialSLine].ElementList[i].transform.DOComplete(true);
        }


        List<T1> newList = new List<T1>(_elementDict[fLine]);
        _elementDict[fLine].Clear();
        _elementDict[fLine] = new List<T1>(_elementDict[sLine]);
        _elementDict[sLine].Clear();
        _elementDict[sLine] = new List<T1>(newList);

        

        Quaternion dialFRot = _dialElementList[dialFLine].transform.localRotation;
        Quaternion dialSRot = _dialElementList[dialSLine].transform.localRotation;

        _dialElementList[dialFLine].transform.rotation = Quaternion.identity;
        _dialElementList[dialSLine].transform.rotation = Quaternion.identity;

        _dialElementList[dialFLine]?.SetRuneList(_elementDict[fLine]);
        _dialElementList[dialSLine]?.SetRuneList(_elementDict[sLine]);

        

        float fOffset = _lineDistanceArray[dialFLine] / _lineDistanceArray[dialSLine];
        for (int i = 0; i < _dialElementList[dialFLine].ElementList.Count; i++)
        {
            if (_dialElementList[dialFLine] != null && _dialElementList[dialFLine].ElementList[i] != null)
            {
                //_dialElementList[dialFLine].ElementList[i].transform.DOComplete();
                _dialElementList[dialFLine].ElementList[i].transform.SetParent(_dialElementList[dialFLine].transform);
                _dialElementList[dialFLine].ElementList[i].transform.localScale = new Vector3(0.1f, 0.1f, 1f);
                _dialElementList[dialFLine].ElementList[i].transform.DOLocalMove(_dialElementList[dialFLine].ElementList[i].transform.localPosition * fOffset, 0.2f);
                //_dialElementList[dialFLine].ElementList[i].transform.localPosition *= fOffset;
            }
        }

        float sOffset = _lineDistanceArray[dialSLine] / _lineDistanceArray[dialFLine];
        for (int i = 0; i < _dialElementList[dialSLine].ElementList.Count; i++)
        {
            if (_dialElementList[dialSLine] != null && _dialElementList[dialSLine].ElementList[i] != null)
            {
                //_dialElementList[dialSLine].ElementList[i].transform.DOComplete();
                _dialElementList[dialSLine].ElementList[i].transform.SetParent(_dialElementList[dialSLine].transform);
                _dialElementList[dialSLine].ElementList[i].transform.localScale = new Vector3(0.1f, 0.1f, 1f);
                _dialElementList[dialSLine].ElementList[i].transform.DOLocalMove(_dialElementList[dialSLine].ElementList[i].transform.localPosition * sOffset, 0.2f);
                //_dialElementList[dialSLine].ElementList[i].transform.localPosition *= sOffset;
            }
        }

        //yield return new WaitForSeconds(0.3f);

        _dialElementList[dialFLine].transform.localRotation = dialSRot;
        _dialElementList[dialSLine].transform.localRotation = dialFRot;

        //RuneSort(true);

        //for (int i = 0; i < _dialElementList.Count; i++)
        //{
        //    _dialElementList[i].transform.rotation = Quaternion.Euler(Vector3.zero);
        //}

        //for (int i = 1; i <= 3; i++)
        //{
        //    RuneLineMove(i, true);
        //}

        //for (int i = 0; i < _dialElementList.Count; i++)
        //{
        //    _dialElementList[i].SelectElement = null;
        //}

        OnLineChange?.Invoke();
    }

    public float GetAngleRadian(float dx, float dy)
    {
        if (dx == 0 && dy == 0)
            return 0;

        if (dy == 0)
        {
            if (dx > 0)
                return 0;
            else
                return Mathf.PI;
        }

        if (dx == 0)
        {
            if (dy == 0)
                return (Mathf.PI / 2);
            else
                return (Mathf.PI / 2) * 3;
        }

        if (dx > 0)
        {
            if (dy > 0)
                return Mathf.Atan(dy / dx);
            else
                return Mathf.Atan(dy / dx) + (2 * Mathf.PI);
        }
        else
            return Mathf.Atan(dy / dx) + Mathf.PI;
    }

    public void RuneLineMove(int line, bool isTween = false)
    {
        if (_elementDict.ContainsKey(line))
        {
            for (int i = 0; i < _elementDict[line].Count; i++)
            {
                float dy = _elementDict[line][i].transform.position.y - this.transform.position.y;
                float dx = _elementDict[line][i].transform.position.x - this.transform.position.x;

                float fAngle = GetAngleRadian(dx, dy);

                float height = Mathf.Sin(fAngle) * _lineDistanceArray[3 - line];
                float width = Mathf.Cos(fAngle) * _lineDistanceArray[3 - line];
                if (isTween)
                {
                    _elementDict[line][i].transform.DOKill();
                    _elementDict[line][i].transform.DOMove(new Vector3(width + this.transform.position.x, height + this.transform.position.y, 0), 0.2f);
                }
                else
                {
                    _elementDict[line][i].transform.position = new Vector3(width + this.transform.position.x, height + this.transform.position.y, 0);
                }

                Vector2 direction = new Vector2(
                    (width + this.transform.position.x) - transform.position.x,
                    (height + this.transform.position.y) - transform.position.y
                );

                float ang = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion angleAxis = Quaternion.AngleAxis(ang - 90f, Vector3.forward);
                _elementDict[line][i].transform.rotation = angleAxis;
            }
        }
    }

    public virtual void SettingDialRune(bool isReset) { }

    public void AddCard(in T1 card, in int tier)
    {
        if (card != null)
        {
            if (_elementDict.ContainsKey(tier))
            {
                _elementDict[tier].Add(card);
            }
            else
            {
                _elementDict.Add(tier, new List<T1> { card });
            }

            RuneSort();
        }
    }

    protected void RuneSort(bool isTween = false)
    {
        for (int i = 1; i <= 3; i++)
        {
            LineCardSort(i, isTween);
        }
    }

    public void LineCardSort(int line, bool isTween = false, float tweenDuration = 0.2f)
    {
        if (_elementDict.ContainsKey(line))
        {
            float angle = -1 * _dialAngle / _elementDict[line].Count;

            for (int i = 0; i < _elementDict[line].Count; i++)
            {
                if (_elementDict[line][i] != null)
                {
                    float radianValue = angle * i + _startAngle;

                    float height = Mathf.Sin(radianValue * Mathf.Deg2Rad) * _lineDistanceArray[3 - line];
                    float width = Mathf.Cos(radianValue * Mathf.Deg2Rad) * _lineDistanceArray[3 - line];
                    if (isTween)
                    {
                        _elementDict[line][i]?.transform.DOComplete();
                        _elementDict[line][i]?.transform.DOMove(new Vector3(width + this.transform.position.x, height + this.transform.position.y, 0), tweenDuration).OnComplete(() =>
                        {
                            for (int i = 0; i < _dialElementList.Count; i++)
                            {
                                _dialElementList[i].SelectElement = _dialElementList[i].SelectElement;
                            }
                        });
                    }
                    else
                    {
                        _elementDict[line][i].transform.position = new Vector3(width + this.transform.position.x, height + this.transform.position.y, 0);
                    }

                    Vector2 direction = new Vector2(
                        (width + this.transform.position.x) - transform.position.x,
                        (height + this.transform.position.y) - transform.position.y
                    );

                    float ang = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    Quaternion angleAxis = Quaternion.AngleAxis(ang - 90f, Vector3.forward);
                    _elementDict[line][i].transform.rotation = angleAxis;
                }
            }

            for (int i = 0; i < _dialElementList.Count; i++)
            {
                _dialElementList[i].SelectElement = _dialElementList[i].SelectElement;
            }
        }
    }

    [Obsolete]
    public Transform RuneTransformBySelectRune(int line, int index)
    {
        List<T1> newList = new List<T1>(_elementDict[line]);

        int count = newList.Count;
        for (int i = 0; i < count; i++)
        {
            if (newList[0] == _dialElementList[3 - line].SelectElement)
                break;
            else
            {
                T1 rune = newList[0];
                newList.RemoveAt(0);
                newList.Add(rune);
            }
        }

        return newList[index].transform;
    }

    public void ResetDial()
    {
        if (_dialElementList == null) return;

        for (int i = 0; i < _dialElementList.Count; i++)
        {
            _dialElementList[i].transform.eulerAngles = Vector3.zero;
        }
    }

    public void AllMagicActive(bool value)
    {
        for (int i = 1; i <= 3; i++)
        {
            if (_elementDict.ContainsKey(i))
            {
                for (int j = 0; j < _elementDict[i].Count; j++)
                {
                    _elementDict[i][j].gameObject.SetActive(false);
                }
            }
        }
    }

    public bool MagicEmpty(bool isAll = true)
    {
        if (isAll)
        {
            return _dialElementList[0].SelectElement == null && _dialElementList[1].SelectElement == null && _dialElementList[2].SelectElement == null;
        }
        else
        {
            return _dialElementList[0].SelectElement == null || _dialElementList[1].SelectElement == null || _dialElementList[2].SelectElement == null;
        }
    }


    public virtual void Attack()
    {
        if (_attackable == false) return;
        if (MagicEmpty() == true) return;

        if (_isAttack == true) return;
        if (_isAttackCondition)
        {
            _isAttack = true;

            StartCoroutine(AttackCoroutine());
            _isAttack = false;
        }
    }

    protected virtual IEnumerator AttackCoroutine()
    {
        yield return null;
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

    public void DialLock()
    {
        _dialElementList.ForEach(x => x.IsDialLock = true);
    }

    public void DialUnlock()
    {
        _dialElementList.ForEach(x => x.IsDialLock = false);
    }

    public void SetAttackable(bool value)
    {
        _attackable = value;
    }
}
