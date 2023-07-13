using DG.Tweening;
using MyBox;
using NativeSerializableDictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T1">??????????????????????대첐???????????????????????????????????????濾??????????????濾?????????????怨쀫뮝嶺????????????????????????ex) BaseRuneUI</typeparam>
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
    [SerializeField]
    protected Dictionary<int, List<T1>> _elementDict;
    [SerializeField]
    protected Dictionary<int, T1[]> _dataDict;
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
    public bool IsAttack { get => _isAttack; set => _isAttack = value; }
    protected virtual bool _isAttackCondition { get; }

    protected virtual void Awake()
    {
        #region Initialization 
        _elementDict = new Dictionary<int, List<T1>>(3);
        for (int i = 1; i <= 3; i++)
        {
            _elementDict.Add(i, new List<T1>(_maxCount * _copyCount));
        }
        _dialElementList = new List<DialElement<T1, T2>>();

        for (int i = 0; i < _lineDistanceArray.Length; i++)
        {
            DialElement<T1, T2> d = this.transform.GetChild(i).GetComponent<DialElement<T1, T2>>();
            d.SetLineID(3 - i);

            _dialElementList.Add(d);
        }

        _dataDict = new Dictionary<int, T1[]>(3);
        for (int i = 1; i <= 3; i++)
        {
            _dataDict.Add(i, new T1[_maxCount]);
        }
        #endregion

        _isAttack = false;
    }

    protected virtual void Start()
    {
        SettingDialRune(true);
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

                    // ???????????????????????????????????????遺얘턁???????猷몄굣????????????????濡?씀?濾????ㅼ굡???????
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

        if (_elementDict.ContainsKey(fLine) == false || _elementDict.ContainsKey(sLine) == false)
        {
            Debug.LogWarning("Not have Key");
            return;
        }

        #region nine version
        //List<T1> newList = new List<T1>(_elementDict[fLine]);
        //_elementDict[fLine].Clear();
        //_elementDict[fLine] = new List<T1>(_elementDict[sLine]);
        //_elementDict[sLine].Clear();
        //_elementDict[sLine] = new List<T1>(newList);

        //int dialFLine = 3 - fLine;
        //int dialSLine = 3 - sLine;
        //Vector3 dailSRot = _dialElementList[dialSLine].transform.eulerAngles;
        //Vector3 dailFRot = _dialElementList[dialFLine].transform.eulerAngles;

        ////_dialElementList[dialFLine].transform.eulerAngles = Vector3.zero;
        ////_dialElementList[dialSLine].transform.eulerAngles = Vector3.zero;


        //Vector3 rot = _dialElementList[dialFLine].transform.eulerAngles;
        //_dialElementList[dialFLine].transform.eulerAngles = _dialElementList[dialSLine].transform.eulerAngles - new Vector3(0, 0, dailSRot.z);
        //_dialElementList[dialSLine].transform.eulerAngles = rot - new Vector3(0, 0, dailFRot.z);

        ////_dialElementList[dialFLine].transform.eulerAngles = dailSRot;
        ////_dialElementList[dialSLine].transform.eulerAngles = dailFRot;

        //_dialElementList[dialFLine]?.SetRuneList(_elementDict[fLine]);
        //_dialElementList[dialSLine]?.SetRuneList(_elementDict[sLine]);

        //if (fLine < sLine)
        //{
        //    rot = _dialElementList[dialFLine].transform.eulerAngles;
        //    _dialElementList[dialFLine].transform.eulerAngles = _dialElementList[dialSLine].transform.eulerAngles;
        //    _dialElementList[dialSLine].transform.eulerAngles = rot;
        //}


        ////T1 rune = _dialElementList[dialFLine].SelectElement;
        ////_dialElementList[dialFLine].SelectElement = _dialElementList[dialSLine].SelectElement;
        ////_dialElementList[dialSLine].SelectElement = rune;



        //float offset = _lineDistanceArray[dialFLine] / _lineDistanceArray[dialSLine];
        //for (int i = 0; i < _dialElementList[dialFLine].ElementList.Count; i++)
        //{
        //    if (_dialElementList[dialFLine] != null && _dialElementList[dialFLine].ElementList[i] != null)
        //    {
        //        _dialElementList[dialFLine].ElementList[i].transform.DOComplete();
        //        _dialElementList[dialFLine].ElementList[i].transform.SetParent(_dialElementList[dialFLine].transform);
        //        _dialElementList[dialFLine].ElementList[i].transform.localScale = new Vector3(0.1f, 0.1f, 1f);
        //        _dialElementList[dialFLine].ElementList[i].transform.DOLocalMove(_dialElementList[dialFLine].ElementList[i].transform.localPosition * offset, 0.2f);
        //    }
        //}

        //offset = _lineDistanceArray[dialSLine] / _lineDistanceArray[dialFLine];
        //for (int i = 0; i < _dialElementList[dialSLine].ElementList.Count; i++)
        //{
        //    if (_dialElementList[dialSLine] != null && _dialElementList[dialSLine].ElementList[i] != null)
        //    {
        //        _dialElementList[dialSLine].ElementList[i].transform.DOComplete();
        //        _dialElementList[dialSLine].ElementList[i].transform.SetParent(_dialElementList[dialSLine].transform);
        //        _dialElementList[dialSLine].ElementList[i].transform.localScale = new Vector3(0.1f, 0.1f, 1f);
        //        _dialElementList[dialSLine].ElementList[i].transform.DOLocalMove(_dialElementList[dialSLine].ElementList[i].transform.localPosition * offset, 0.2f);
        //    }
        //}


        ////Quaternion rot = _dialElementList[dialFLine].transform.rotation;
        ////_dialElementList[dialFLine].transform.rotation = _dialElementList[dialSLine].transform.rotation;
        ////_dialElementList[dialSLine].transform.rotation = rot;

        ////RuneSort(true);

        ////for (int i = 0; i < _dialElementList.Count; i++)
        ////{
        ////    _dialElementList[i].transform.rotation = Quaternion.Euler(Vector3.zero);
        ////}

        ////for (int i = 1; i <= 3; i++)
        ////{
        ////    RuneLineMove(i, true);
        ////}

        //for (int i = 0; i < _dialElementList.Count; i++)
        //{
        //    _dialElementList[i].SelectElement = null;
        //}
        #endregion

        #region new version
        int dialFLine = 3 - fLine;
        int dialSLine = 3 - sLine;

        //Quaternion dialFRot = _dialElementList[dialFLine].transform.rotation;
        //Quaternion dialSRot = _dialElementList[dialSLine].transform.rotation;
        Vector3 dialFRot = _dialElementList[dialFLine].transform.eulerAngles;
        Vector3 dialSRot = _dialElementList[dialSLine].transform.eulerAngles;

        _dialElementList[dialFLine].transform.rotation = Quaternion.Euler(Vector3.zero);
        _dialElementList[dialSLine].transform.rotation = Quaternion.Euler(Vector3.zero);

        Sequence seq = DOTween.Sequence();
        float offset = _lineDistanceArray[dialSLine] / _lineDistanceArray[dialFLine];
        for (int i = 0; i < _dialElementList[dialFLine].ElementList.Count; i++)
        {
            if (_dialElementList[dialFLine] != null && _dialElementList[dialFLine].ElementList[i] != null)
            {
                _dialElementList[dialFLine].ElementList[i].transform.DOComplete();
                _dialElementList[dialFLine].ElementList[i].transform.SetParent(_dialElementList[dialSLine].transform);
                _dialElementList[dialFLine].ElementList[i].transform.localScale = new Vector3(0.1f, 0.1f, 1f);
                seq.Join(_dialElementList[dialFLine].ElementList[i].transform.DOLocalMove(_dialElementList[dialFLine].ElementList[i].transform.localPosition * offset, 0.2f));
            }
        }

        offset = _lineDistanceArray[dialFLine] / _lineDistanceArray[dialSLine];
        for (int i = 0; i < _dialElementList[dialSLine].ElementList.Count; i++)
        {
            if (_dialElementList[dialSLine] != null && _dialElementList[dialSLine].ElementList[i] != null)
            {
                _dialElementList[dialSLine].ElementList[i].transform.DOComplete();
                _dialElementList[dialSLine].ElementList[i].transform.SetParent(_dialElementList[dialFLine].transform);
                _dialElementList[dialSLine].ElementList[i].transform.localScale = new Vector3(0.1f, 0.1f, 1f);
                seq.Join(_dialElementList[dialSLine].ElementList[i].transform.DOLocalMove(_dialElementList[dialSLine].ElementList[i].transform.localPosition * offset, 0.2f));
            }
        }

        seq.AppendCallback(() =>
        {
            List<T1> newList = new List<T1>(_elementDict[fLine]);
            _elementDict[fLine].Clear();
            _elementDict[fLine] = new List<T1>(_elementDict[sLine]);
            _elementDict[sLine].Clear();
            _elementDict[sLine] = new List<T1>(newList);

            //Quaternion rot = _dialElementList[dialFLine].transform.rotation;
            //_dialElementList[dialFLine].transform.rotation = _dialElementList[dialSLine].transform.rotation;
            //_dialElementList[dialSLine].transform.rotation = rot;

            _dialElementList[dialFLine]?.SetRuneList(_elementDict[fLine]);
            _dialElementList[dialSLine]?.SetRuneList(_elementDict[sLine]);



            //Quaternion inverseDialFRot = Quaternion.Inverse(dialFRot);
            //Quaternion inverseDialSRot = Quaternion.Inverse(dialSRot);

            Debug.Log("Change");
            Debug.Log(_dialElementList[dialFLine].transform.rotation);
            Debug.Log(Quaternion.Euler(dialSRot));
            _dialElementList[dialFLine].transform.rotation = Quaternion.Euler(dialSRot); // Quaternion.Euler(inverseDialSRot.x, inverseDialSRot.y, inverseDialSRot.z/* + (306f / _elementDict[fLine].Count)*/);
            Debug.Log(_dialElementList[dialFLine].transform.rotation);
            Debug.Log(Quaternion.Euler(dialSRot));

            Debug.Log("Choice");
            Debug.Log(_dialElementList[dialSLine].transform.rotation);
            Debug.Log(Quaternion.Euler(dialFRot));
            _dialElementList[dialSLine].transform.rotation = Quaternion.Euler(dialFRot);
            Debug.Log(_dialElementList[dialSLine].transform.rotation);
            Debug.Log(Quaternion.Euler(dialFRot));
            //dialSRot.z += 360f / _dialElementList[dialSLine].ElementList.Count;
            //inverseDialSRot.z *= 2;

            for (int i = 0; i < _dialElementList.Count; i++)
            {
                _dialElementList[i].SelectElement = null;
            }
        });
        #endregion

        #region Third version
        //int dialFLine = 3 - fLine;
        //int dialSLine = 3 - sLine;

        //float offset = _lineDistanceArray[dialFLine] / _lineDistanceArray[dialSLine];
        //for (int i = 0; i < _dialElementList[dialFLine].ElementList.Count; i++)
        //{
        //    if (_dialElementList[dialFLine] != null && _dialElementList[dialFLine].ElementList[i] != null)
        //    {
        //        _dialElementList[dialFLine].ElementList[i].transform.DOComplete();
        //        _dialElementList[dialFLine].ElementList[i].transform.localScale = new Vector3(0.1f, 0.1f, 1f);
        //        _dialElementList[dialFLine].ElementList[i].transform.DOLocalMove(_dialElementList[dialFLine].ElementList[i].transform.localPosition * offset, 0.2f).OnComplete(() =>
        //        {
        //            _dialElementList[dialFLine].ElementList[i].transform.localPosition = _dialElementList[dialFLine].ElementList[i].transform.localPosition / offset;
        //        });
        //    }
        //}

        //offset = _lineDistanceArray[dialSLine] / _lineDistanceArray[dialFLine];
        //for (int i = 0; i < _dialElementList[dialSLine].ElementList.Count; i++)
        //{
        //    if (_dialElementList[dialSLine] != null && _dialElementList[dialSLine].ElementList[i] != null)
        //    {
        //        _dialElementList[dialSLine].ElementList[i].transform.DOComplete();
        //        _dialElementList[dialSLine].ElementList[i].transform.localScale = new Vector3(0.1f, 0.1f, 1f);
        //        _dialElementList[dialSLine].ElementList[i].transform.DOLocalMove(_dialElementList[dialSLine].ElementList[i].transform.localPosition * offset, 0.2f).OnComplete(() =>
        //        {
        //            _dialElementList[dialSLine].ElementList[i].transform.localPosition = _dialElementList[dialSLine].ElementList[i].transform.localPosition / offset;
        //        });
        //    }
        //}

        //for(int i = 0; i < _dialElementList[dialSLine].ElementList.Count; i++)
        //{

        //}

        #endregion

        #region Data Change
        //int dialFLine = 3 - fLine;
        //int dialSLine = 3 - sLine;

        //Sequence seq = DOTween.Sequence();

        //// ??由????쇱뵠??깆벥 揶쏄낮猷꾤몴??λ뜃由????뽱룖????椰?揶쏆늿?????륁㉦?????ワ쭪?

        //float offset = _lineDistanceArray[dialSLine] / _lineDistanceArray[dialFLine];
        //for (int i = 0; i < _dialElementList[dialFLine].ElementList.Count; i++)
        //{
        //    if (_dialElementList[dialFLine] != null && _dialElementList[dialFLine].ElementList[i] != null)
        //    {
        //        _dialElementList[dialFLine].ElementList[i].transform.DOComplete();
        //        _dialElementList[dialFLine].ElementList[i].transform.SetParent(_dialElementList[dialSLine].transform);
        //        _dialElementList[dialFLine].ElementList[i].transform.localScale = new Vector3(0.1f, 0.1f, 1f);
        //        seq.Join(_dialElementList[dialFLine].ElementList[i].transform.DOLocalMove(_dialElementList[dialFLine].ElementList[i].transform.localPosition * offset, 0.2f));
        //    }
        //}

        //offset = _lineDistanceArray[dialFLine] / _lineDistanceArray[dialSLine];
        //for (int i = 0; i < _dialElementList[dialSLine].ElementList.Count; i++)
        //{
        //    if (_dialElementList[dialSLine] != null && _dialElementList[dialSLine].ElementList[i] != null)
        //    {
        //        _dialElementList[dialSLine].ElementList[i].transform.DOComplete();
        //        _dialElementList[dialSLine].ElementList[i].transform.SetParent(_dialElementList[dialFLine].transform);
        //        _dialElementList[dialSLine].ElementList[i].transform.localScale = new Vector3(0.1f, 0.1f, 1f);
        //        seq.Join(_dialElementList[dialSLine].ElementList[i].transform.DOLocalMove(_dialElementList[dialSLine].ElementList[i].transform.localPosition * offset, 0.2f));
        //    }
        //}
        //seq.AppendCallback(() =>
        //{
        //    //List<T1> newList = new List<T1>(_elementDict[fLine]);
        //    //_elementDict[fLine].Clear();
        //    //_elementDict[fLine] = new List<T1>(_elementDict[sLine]);
        //    //_elementDict[sLine].Clear();
        //    //_elementDict[sLine] = new List<T1>(newList);



        //    T1[] newArray = (T1[])_dataDict[fLine].Clone();
        //    //_dataDict[fLine] = (T1[])_dataDict[sLine].Clone();
        //    _dataDict[fLine].CopyTo(_dataDict[sLine], 0);
        //    //_dataDict[sLine] = (T1[])newArray.Clone();
        //    _dataDict[sLine].CopyTo(newArray, 0);

        //    for (int i = 0; i < _elementDict[fLine].Count; i++)
        //    {
        //        _elementDict[fLine][i] = _dataDict[fLine][i % _maxCount];
        //    }


        //    for (int i = 0; i < _dataDict[sLine].Length; i++)
        //    {
        //        _elementDict[sLine][i] = _dataDict[sLine][i % _maxCount];
        //    }

        //    _dialElementList[dialFLine]?.SetRuneList(_elementDict[fLine]);
        //    _dialElementList[dialSLine]?.SetRuneList(_elementDict[sLine]);

        //    //for (int i = 0; i < _elementDict[fLine].Count; i++)
        //    //{
        //    //    Managers.Resource.Destroy(_elementDict[fLine][i].gameObject);
        //    //}

        //    //for (int i = 0; i < _elementDict[sLine].Count; i++)
        //    //{
        //    //    Managers.Resource.Destroy(_elementDict[sLine][i].gameObject);
        //    //}
        //});

        #endregion
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
}
