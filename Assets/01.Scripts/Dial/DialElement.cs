using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DialState
{
    None,
    Rotate,
    Drag,
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="T1">????μ떜媛?걫??곸쁼筌??????????????????μ떜媛?걫????耀붾굝?????????붾눀??????곹뜢????????ш내?℡ㅇ???????ex) BaseRuneUI</typeparam>
/// <typeparam name="T2">T1???????????ex) BaseRune</typeparam>
public class DialElement<T1, T2> : MonoBehaviour where T1 : MonoBehaviour where T2 : class
{
    protected Dial<T1, T2> _dial;
    private SpriteRenderer _lineSpriteRenderer;
    private SpriteRenderer _textSpriteRenderer;
    [SerializeField]
    private Sprite _textSprite;
    [SerializeField]
    private Sprite _glowTextSprite;

    #region Drag Parameta
    protected float _rotDamp = 3;
    protected Vector3 _touchPos, _offset;

    [SerializeField]
    protected bool _isUseRotateOffset;

    [SerializeField]
    protected float _inDistance;
    public float InDistance => _inDistance;
    [SerializeField]
    protected float _outDistance;
    public float OutDistance => _outDistance;
    #endregion

    protected int _fingerID = -1;
    public int FingerID { get => _fingerID; set => _fingerID = value; }

    protected int _lineID = -1;

    #region Element Parameta
    protected List<T1> _elementList = new List<T1>();
    public List<T1> ElementList => _elementList;
    protected T1 _selectElement;
    public virtual T1 SelectElement
    {
        get => _selectElement;
        set => _selectElement = value;
    }
    #endregion

    protected int _selectIndex = -1;

    [SerializeField, Range(0f, 90f)]
    protected float _selectOffset;

    #region Line Swap Parameta
    [SerializeField]
    private bool _isUsingLineSwap = false;
    public bool IsUsingLineSwap => _isUsingLineSwap;
    protected bool _isTouchDown = false;
    public bool IsTouchDown { get => _isTouchDown; set { _isTouchDown = value; _touchDownTimer = 0f; } }
    protected DialState _dialState = DialState.None;
    public DialState DialState { get => _dialState; set => _dialState = value; }
    [SerializeField]
    protected float _dragTouchTime = 3f;

    protected float _touchDownTimer = 0f;
    #endregion

    public bool IsGlow
    {
        set
        {
            if (value == true)
            {
                _textSpriteRenderer.sprite = _glowTextSprite;
            }
            else
            {
                _textSpriteRenderer.sprite = _textSprite;
            }
        }
    }

    protected virtual bool _isAttackCondition { get; } = true;
    protected virtual bool _isRotateAdditionalCondition { get; } = true;

    protected virtual void Awake()
    {
        _dial = GetComponentInParent<Dial<T1, T2>>();
        _lineSpriteRenderer = transform.Find("LineVisualSprite").GetComponent<SpriteRenderer>();
        _textSpriteRenderer = transform.Find("TextVisualSprite").GetComponent<SpriteRenderer>();

        transform.rotation = Quaternion.Euler(Vector3.zero);
        IsGlow = false;
    }

    protected virtual void OnEnable()
    {
        _dial.ResetDial();
    }

    protected virtual void Start()
    {
        #region Add Event
        Managers.Swipe.AddAction(SwipeType.TouchBegan, (touch) =>
        {
            float inputDistance = Vector2.Distance(Define.MainCam.ScreenToWorldPoint(Managers.Swipe.TouchBeganPos), (Vector2)this.transform.position);
            if (inputDistance >= _inDistance && inputDistance <= _outDistance)
            {
                if (_isTouchDown == true) return;

                _fingerID = touch.fingerId;
                _isTouchDown = true;

                _touchPos = touch.position;
            }
            _dialState = DialState.Rotate;
        });

        Managers.Swipe.AddAction(SwipeType.TouchMove, (touch) =>
        {
            switch (_dialState)
            {
                case DialState.None:
                    break;
                case DialState.Rotate:
                    RotateMagicCircle();
                    break;
                case DialState.Drag:
                    break;
            }
        });

        Managers.Swipe.AddAction(SwipeType.TouchEnd, (touch) =>
        {
            if (_isTouchDown == true)
            {
                _fingerID = -1;
                _isTouchDown = false;
                _dialState = DialState.None;
                _touchDownTimer = 0f;
                _dial.SelectDialElement(null);
                _dial.AllMagicCircleGlow(false);

                ElementMoveInLine();
            }
        });
        #endregion
    }

    protected virtual void Update()
    {
        UpdateSelectElement();

        //RotateMagicCircle();

        if (_isTouchDown == true && _isUsingLineSwap == true)
        {
            _touchDownTimer += Time.deltaTime;

            if (_touchDownTimer > _dragTouchTime)
            {
                _dialState = DialState.Drag;
                _dial.SelectDialElement(this);
            }
        }
    }

    private void RotateMagicCircle()
    {
        if (_isTouchDown && _isRotateAdditionalCondition)
        {
            _offset = ((Vector3)Input.GetTouch(_fingerID).position - _touchPos);

            Vector3 rot = transform.eulerAngles;

            float temp = Input.GetTouch(_fingerID).position.x > Screen.width / 2 ? _offset.x - _offset.y : _offset.x + _offset.y;

            if (Mathf.Abs(_offset.x) > Mathf.Abs(_offset.y))
            {
                if (_offset.x > 0)
                    temp = Mathf.Clamp(temp, 0, _offset.x);
                else
                    temp = Mathf.Clamp(temp, _offset.x, 0);
            }
            else
            {
                if (_offset.y > 0)
                    temp = Mathf.Clamp(temp, -_offset.y, _offset.y);
                else
                    temp = Mathf.Clamp(temp, _offset.y, -_offset.y);
            }

            rot.z += -1 * temp / _rotDamp;

            transform.rotation = Quaternion.Euler(rot);
            _touchPos = Input.GetTouch(_fingerID).position;

            //Debug.Log(temp / _rotDamp);
            if (Mathf.Abs(temp / _rotDamp) >= 3F)
            {
                _touchDownTimer = 0f;
            }

            _dial.SelectDialElement(null);
        }
    }

    private void UpdateSelectElement()
    {
        if (_elementList.Count > 0 && _isRotateAdditionalCondition)
        {
            float oneDinstance = _dial.DialAngle / _elementList.Count;
            bool inBoolean = (transform.eulerAngles.z % oneDinstance) <= _selectOffset;
            bool outBoolean = (oneDinstance - (transform.eulerAngles.z % oneDinstance)) <= _selectOffset;
            if (inBoolean && transform.eulerAngles.z >= 0)
            {
                int index = (int)(transform.eulerAngles.z / oneDinstance) % (_elementList.Count);
                ChangeSelectElement(index);
            }
            else if (outBoolean && transform.eulerAngles.z <= 360)
            {
                int index = (int)(transform.eulerAngles.z / oneDinstance) % (_elementList.Count);
                index = (index + 1) % (_elementList.Count);
                ChangeSelectElement(index);
            }
            else
            {
                ChangeSelectElement(-1);
                if (_isTouchDown == true)
                {
                    OnSelectElementAction();
                }
            }
        }
    }

    protected virtual void ChangeSelectElement(int index)
    {
        
    }

    [Obsolete]
    public void MoveRune(int index, bool isLeft = true)
    {
        if (isLeft == true)
        {
            T1 rune = _elementList[index];

            float radianValue = 0f;
            if (index - 1 < 0)
            {
                radianValue = (_dial.DialAngle / _elementList.Count) * (index - 1) + (_dial.StartAngle * Mathf.Deg2Rad);
            }
            else
            {
                radianValue = (_dial.DialAngle - (_dial.DialAngle / _elementList.Count)) + (_dial.StartAngle * Mathf.Deg2Rad);
            }

            float height = Mathf.Sin(radianValue) * _dial.LineDistanceArray[3 - _lineID];
            float width = Mathf.Cos(radianValue) * _dial.LineDistanceArray[3 - _lineID];
            _elementList[index].transform.position = new Vector3(width + this.transform.position.x, height + this.transform.position.y, 0);

            Vector2 direction = new Vector2(
                _elementList[index].transform.position.x - transform.position.x,
                _elementList[index].transform.position.y - transform.position.y
            );

            float ang = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion angleAxis = Quaternion.AngleAxis(ang - 90f, Vector3.forward);
            _elementList[index].transform.rotation = angleAxis;

            _elementList.RemoveAt(index);
            _elementList.Insert(0, rune);
        }
        else
        {
            T1 rune = _elementList[index];

            float radianValue = (_dial.DialAngle / _elementList.Count) * (index + 1) + (_dial.StartAngle * Mathf.Deg2Rad);

            float height = Mathf.Sin(radianValue) * _dial.LineDistanceArray[3 - _lineID];
            float width = Mathf.Cos(radianValue) * _dial.LineDistanceArray[3 - _lineID];
            _elementList[index].transform.position = new Vector3(width + this.transform.position.x, height + this.transform.position.y, 0);

            Vector2 direction = new Vector2(
                _elementList[index].transform.position.x - transform.position.x,
                _elementList[index].transform.position.y - transform.position.y
            );

            float ang = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion angleAxis = Quaternion.AngleAxis(ang - 90f, Vector3.forward);
            _elementList[index].transform.rotation = angleAxis;

            _elementList.RemoveAt(index);
            _elementList.Add(rune);
        }
    }

    public void SetLineID(int id)
    {
        _lineID = id;
    }

    public int GetLineID()
    {
        return _lineID;
    }

    public void AddRuneList(in T1 rune)
    {
        _elementList.Add(rune);
    }

    public void SetRuneList(in List<T1> list)
    {
        _elementList.Clear();
        _elementList = new List<T1>(list);
    }

    public void ResetRuneList()
    {
        _elementList.Clear();
    }

    public virtual void Attack()
    {

    }

    public void ElementMoveInLine()
    {
        if (_elementList.Count > 0 && _isRotateAdditionalCondition)
        {
            if (_isUseRotateOffset)
            {
                float oneDinstance = _dial.DialAngle / _elementList.Count;
                bool inBoolean = (transform.eulerAngles.z % oneDinstance) <= _selectOffset;
                bool outBoolean = (oneDinstance - (transform.eulerAngles.z % oneDinstance)) <= _selectOffset;
                if (inBoolean)
                {
                    int index = (int)(transform.eulerAngles.z / oneDinstance) % (_elementList.Count);
                    DOTween.To(() => transform.eulerAngles, x => transform.eulerAngles = x,
                        new Vector3(0, 0, ((int)(transform.eulerAngles.z / oneDinstance)) * oneDinstance),
                        0.3f
                    ).OnComplete(() =>
                    {
                        if (_selectElement != null)
                        {
                            OnSelectElementAction();
                            //Define.DialScene?.CardDescPopup(_selectElement.Rune);
                        }
                    });
                }
                else if (outBoolean)
                {
                    int index = ((int)(transform.eulerAngles.z / oneDinstance) + 1) % (_elementList.Count);
                    index = (index + 1) % _elementList.Count;
                    DOTween.To(
                        () => transform.eulerAngles,
                        x => transform.eulerAngles = x,
                        new Vector3(0, 0, ((int)(transform.eulerAngles.z / oneDinstance)) * oneDinstance + _dial.StartAngle),
                        0.3f
                    ).OnComplete(() =>
                    {
                        if (_selectElement != null)
                        {
                            OnSelectElementAction();
                            //Define.DialScene?.CardDescPopup(_selectElement.Rune);
                        }
                    });
                }
            }
            else
            {
                float oneDinstance = _dial.DialAngle / _elementList.Count;
                int index = (int)(transform.eulerAngles.z / oneDinstance) % (_elementList.Count);

                float distance = transform.eulerAngles.z % oneDinstance;
                if (distance >= oneDinstance / 2f)
                {
                    transform.DORotate(new Vector3(0, 0, ((index + 1) % _elementList.Count * oneDinstance)), 0.3f, RotateMode.Fast)
                        .OnComplete(() =>
                        {
                            if (_selectElement != null)
                            {
                                OnSelectElementAction();
                                //Define.DialScene?.CardDescPopup(_selectElement.Rune);
                            }
                        });
                }
                else
                {
                    transform.DORotate(new Vector3(0, 0, ((index) * oneDinstance)), 0.3f, RotateMode.Fast)
                        .OnComplete(() =>
                        {

                            if (_selectElement != null)
                            {
                                OnSelectElementAction();
                                //Define.DialScene?.CardDescPopup(_selectElement.Rune);
                            }
                        });
                }
            }
        }
    }

    protected virtual void OnSelectElementAction()
    {

    }

    public void ElementListSort()
    {
        if (_selectElement == null) return;

        while (_elementList[1] != _selectElement)
        {
            T1 rune = _elementList[0];
            _elementList.RemoveAt(0);
            _elementList.Add(rune);
            _selectIndex = (_selectIndex - 1) % _elementList.Count;
            SelectElement = _elementList[_selectIndex];
            InfiniteLoopDetector.Run();
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, _inDistance);
        Gizmos.DrawWireSphere(this.transform.position, _outDistance);
        Gizmos.color = Color.white;
    }
#endif
}
