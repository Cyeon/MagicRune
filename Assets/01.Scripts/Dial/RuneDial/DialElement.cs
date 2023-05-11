using DG.Tweening;
using MyBox;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum DialState
{
    None,
    Rotate,
    Drag,
}

public class DialElement : MonoBehaviour
{
    private Dial _dial;
    private SpriteRenderer _lineSpriteRenderer;
    private SpriteRenderer _textSpriteRenderer;
    [SerializeField]
    private Sprite _textSprite;
    [SerializeField]
    private Sprite _glowTextSprite;

    #region Swipe Parameta
    private Vector2 touchBeganPos;
    private Vector2 touchEndedPos;
    private Vector2 touchDif;
    [SerializeField]
    private float swipeSensitivity = 5;
    #endregion

    #region Drag Parameta
    private float _rotDamp = 3;
    private Vector3 _touchPos, _offset;

    [SerializeField]
    private bool _isUseRotateOffset;

    [SerializeField]
    private float _inDistance;
    [SerializeField]
    private float _outDistance;
    #endregion

    [SerializeField]
    private float _runePoolOffset = 5f;

    private int _fingerID = -1;

    private int _lineID = -1;

    private List<BaseRuneUI> _runeList;
    private BaseRuneUI _selectCard;
    public BaseRuneUI SelectCard
    {
        get
        {
            //if (_selectCard == null)
            //    _selectCard = _runeList[0];
            return _selectCard;
        }
        set
        {
            if (value == null)
            {
                if (_selectCard != null)
                {
                    //_selectCard.SetActiveOutline(OutlineType.Default);
                    _selectCard.RuneColor(new Color(0.26f, 0.26f, 0.26f, 1f));
                }
                _selectCard = value;
            }
            else
            {
                if (_selectCard != null)
                {
                    //_selectCard.SetActiveOutline(OutlineType.Default);
                    _selectCard.RuneColor(new Color(0.26f, 0.26f, 0.26f, 1f));
                }

                _selectCard = value;

                //_selectCard.SetActiveOutline(OutlineType.Cyan);
                _selectCard.RuneColor(Color.white);

            }
            _dial.CheckResonance();
        }
    }

    [SerializeField, Range(0f, 90f)]
    private float _selectOffset;
    private bool _isTouchDown = false;
    private DialState _dialState = DialState.None;

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


    private void Awake()
    {
        _dial = GetComponentInParent<Dial>();
        _lineSpriteRenderer = transform.Find("LineVisualSprite").GetComponent<SpriteRenderer>();
        _textSpriteRenderer = transform.Find("TextVisualSprite").GetComponent<SpriteRenderer>();
        _runeList = new List<BaseRuneUI>();
        //_spriteRenderer.alphaHitTestMinimumThreshold = 0.04f;
    }

    private void Start()
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);
        IsGlow = false;
    }

    private void Update()
    {
        Swipe1();

        UpdateRunePanel();
        //RotateMagicCircle();
    }

    private void RotateMagicCircle()
    {
        if (_isTouchDown && BattleManager.Instance.IsPlayerTurn())
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
        }
    }

    private void UpdateRunePanel()
    {
        if (_runeList.Count > 0 && BattleManager.Instance.IsPlayerTurn())
        {
            float oneDinstance = _dial.RuneAngle / _runeList.Count;
            bool inBoolean = (transform.eulerAngles.z % oneDinstance) <= _selectOffset;
            bool outBoolean = (oneDinstance - (transform.eulerAngles.z % oneDinstance)) <= _selectOffset;
            if (inBoolean && transform.eulerAngles.z >= 0)
            {
                int index = (int)(transform.eulerAngles.z / oneDinstance) % (_runeList.Count);
                if (_runeList[index].Rune.IsCoolTime == false)
                {
                    SelectCard = _runeList[index];
                    if (_isTouchDown == true)
                    {
                        if (_selectCard != null)
                        {
                            Define.DialScene?.CardDescPopup(_selectCard.Rune);
                        }
                    }
                }
            }
            else if (outBoolean && transform.eulerAngles.z <= 360)
            {
                int index = (int)(transform.eulerAngles.z / oneDinstance) % (_runeList.Count);
                index = (index + 1) % (_runeList.Count);

                if (_runeList[index].Rune.IsCoolTime == false)
                {
                    SelectCard = _runeList[index];
                    if (_isTouchDown == true)
                    {
                        if (_selectCard != null)
                        {
                            Define.DialScene?.CardDescPopup(_selectCard.Rune);
                        }
                    }
                }
            }
            else
            {
                SelectCard = null;
                if (_isTouchDown == true)
                {
                    BaseRune rune = SelectCard == null ? null : SelectCard.Rune;
                    Define.DialScene?.CardDescPopup(rune);
                }
            }
        }
    }

    [Obsolete]
    public void MoveRune(int index, bool isLeft = true)
    {
        if (isLeft == true)
        {
            BaseRuneUI rune = _runeList[index];

            float radianValue = 0f;
            if (index - 1 < 0)
            {
                radianValue = (_dial.RuneAngle / _runeList.Count) * (index - 1) + (_dial.StartAngle * Mathf.Deg2Rad);
            }
            else
            {
                radianValue = (_dial.RuneAngle - (_dial.RuneAngle / _runeList.Count)) + (_dial.StartAngle * Mathf.Deg2Rad);
            }

            float height = Mathf.Sin(radianValue) * _dial.LineDistanceArray[3 - _lineID];
            float width = Mathf.Cos(radianValue) * _dial.LineDistanceArray[3 - _lineID];
            _runeList[index].transform.position = new Vector3(width + this.transform.position.x, height + this.transform.position.y, 0);

            Vector2 direction = new Vector2(
                _runeList[index].transform.position.x - transform.position.x,
                _runeList[index].transform.position.y - transform.position.y
            );

            float ang = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion angleAxis = Quaternion.AngleAxis(ang - 90f, Vector3.forward);
            _runeList[index].transform.rotation = angleAxis;

            _runeList.RemoveAt(index);
            _runeList.Insert(0, rune);
        }
        else
        {
            BaseRuneUI rune = _runeList[index];

            float radianValue = (_dial.RuneAngle / _runeList.Count) * (index + 1) + (_dial.StartAngle * Mathf.Deg2Rad);

            float height = Mathf.Sin(radianValue) * _dial.LineDistanceArray[3 - _lineID];
            float width = Mathf.Cos(radianValue) * _dial.LineDistanceArray[3 - _lineID];
            _runeList[index].transform.position = new Vector3(width + this.transform.position.x, height + this.transform.position.y, 0);

            Vector2 direction = new Vector2(
                _runeList[index].transform.position.x - transform.position.x,
                _runeList[index].transform.position.y - transform.position.y
            );

            float ang = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion angleAxis = Quaternion.AngleAxis(ang - 90f, Vector3.forward);
            _runeList[index].transform.rotation = angleAxis;

            _runeList.RemoveAt(index);
            _runeList.Add(rune);
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

    public void AddRuneList(BaseRuneUI rune)
    {
        _runeList.Add(rune);
    }

    public void SetRuneList(List<BaseRuneUI> list)
    {
        _runeList = new List<BaseRuneUI>(list);
    }

    public void ResetRuneList()
    {
        _runeList.Clear();
    }

    public void Attack()
    {
        if (BattleManager.Instance.Enemy.IsDie == false && _selectCard != null)
        {
            if (SelectCard.Rune.AbilityCondition())
            {
                SelectCard.Rune.AbilityAction();

                SelectCard = null;
            }
        }
    }

    public void Swipe1()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchBeganPos = touch.position;

                    float inputDistance = Vector2.Distance(Define.MainCam.ScreenToWorldPoint(touchBeganPos), (Vector2)this.transform.position);
                    if (inputDistance >= _inDistance && inputDistance <= _outDistance)
                    {
                        if (_isTouchDown == true) return;

                        _fingerID = touch.fingerId;
                        _isTouchDown = true;

                        _touchPos = touch.position;
                    }
                    break;
                case TouchPhase.Moved:

                    if (touch.deltaTime >= 0.3f)
                    {
                        _dialState = DialState.Drag;
                    }
                    else
                    {
                        _dialState = DialState.Rotate;
                    }

                    switch (_dialState)
                    {
                        case DialState.None:
                            break;
                        case DialState.Rotate:
                            RotateMagicCircle();
                            break;
                        case DialState.Drag:
                            Debug.Log("라인 체인지");
                            break;
                    }
                    break;
                case TouchPhase.Ended:
                    if (_isTouchDown == true)
                    {
                        _fingerID = -1;
                        _isTouchDown = false;

                        if (_runeList.Count > 0 && BattleManager.Instance.IsPlayerTurn())
                        {
                            if (_isUseRotateOffset)
                            {
                                float oneDinstance = _dial.RuneAngle / _runeList.Count;
                                bool inBoolean = (transform.eulerAngles.z % oneDinstance) <= _selectOffset;
                                bool outBoolean = (oneDinstance - (transform.eulerAngles.z % oneDinstance)) <= _selectOffset;
                                if (inBoolean)
                                {
                                    int index = (int)(transform.eulerAngles.z / oneDinstance) % (_runeList.Count);
                                    DOTween.To(
                                        () => transform.eulerAngles,
                                        x => transform.eulerAngles = x,
                                        new Vector3(0, 0, ((int)(transform.eulerAngles.z / oneDinstance)) * oneDinstance),
                                        0.3f
                                    ).OnComplete(() =>
                                    {
                                        if (_selectCard != null) { Define.DialScene?.CardDescPopup(_selectCard.Rune); }
                                    });
                                }
                                else if (outBoolean)
                                {
                                    int index = ((int)(transform.eulerAngles.z / oneDinstance) + 1) % (_runeList.Count);
                                    index = (index + 1) % _runeList.Count;
                                    DOTween.To(
                                        () => transform.eulerAngles,
                                        x => transform.eulerAngles = x,
                                        new Vector3(0, 0, ((int)(transform.eulerAngles.z / oneDinstance)) * oneDinstance + _dial.StartAngle),
                                        0.3f
                                    ).OnComplete(() =>
                                    {
                                        if (_selectCard != null) { Define.DialScene?.CardDescPopup(_selectCard.Rune); }
                                    });
                                }
                            }
                            else
                            {
                                float oneDinstance = _dial.RuneAngle / _runeList.Count;
                                int index = (int)(transform.eulerAngles.z / oneDinstance) % (_runeList.Count);

                                float distance = transform.eulerAngles.z % oneDinstance;
                                if (distance >= oneDinstance / 2f)
                                {
                                    transform.DORotate(new Vector3(0, 0, ((index + 1) % _runeList.Count * oneDinstance)), 0.3f, RotateMode.Fast)
                                        .OnComplete(() =>
                                        {
                                            if (_selectCard != null) { Define.DialScene?.CardDescPopup(_selectCard.Rune); }
                                        });
                                }
                                else
                                {
                                    transform.DORotate(new Vector3(0, 0, ((index) * oneDinstance)), 0.3f, RotateMode.Fast)
                                        .OnComplete(() =>
                                        {
                                            if (_selectCard != null) { Define.DialScene?.CardDescPopup(_selectCard.Rune); }
                                        });
                                }
                            }

                        }
                        _dialState = DialState.None;
                    }
                    break;
            }
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