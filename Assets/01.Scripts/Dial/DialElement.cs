using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialElement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IEndDragHandler, IBeginDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Dial _dial;
    private Image _image;

    
    [SerializeField]
    private int _selectCount = 3;
    [SerializeField]
    private float _dragDistance = 800;

    #region Swipe Parameta
    private Vector2 touchBeganPos;
    private Vector2 touchEndedPos;
    private Vector2 touchDif;
    [SerializeField]
    private float swipeSensitivity = 5;
    #endregion

    #region Drag Parameta
    private float _currentSpeed;
    [SerializeField]
    private float _rotDamp = 3;
    [SerializeField]
    private float _rotAcc = 0.5f;
    [SerializeField]
    private float _rotDeAcc = 0.5f;
    private Vector2 _moveDirection = Vector2.zero;
    #endregion

    private int _fingerID = -1;

    private bool _isSelect = false;
    public bool IsSelect { get => _isSelect; set => _isSelect = value; }
    private bool _isAllSelect = false;
    private bool _isPointerIn = false;

    private List<TestCard> _selectCardList;

    public void OnPointerDown(PointerEventData eventData)
    {
        _fingerID = eventData.pointerId;

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _fingerID = -1;
        //_dial.EditSelectArea(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        StopAllCoroutines();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // ¸¶¿ì½º Æ÷ÀÎÅÍ°¡ ¸¶¹ýÁø À§¿¡ ÀÏ¶§
        if (_isPointerIn == true)
        {
            //if (_currentSpeed < _rotDamp)
            //{
            //    _currentSpeed += _rotAcc;
            //}

            Movement(eventData.delta.x > 0 ? 1 : -1);
            Vector3 rot = _image.transform.eulerAngles;
            rot.z += -1 * _moveDirection.x * _currentSpeed/* * Time.deltaTime*/;
            _image.transform.rotation = Quaternion.Lerp(_image.transform.rotation, Quaternion.Euler(rot), 0.7f);
            Debug.Log(eventData.delta.x);
        }
        //else
        //{
        //    StartCoroutine(EndDrag(eventData));
        //}
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        StartCoroutine(EndDrag(eventData));
        //Movement(eventData.delta.x > 0 ? 1 : -1);
        //Vector3 rot = _image.transform.eulerAngles;
        //rot.z += -1 * _currentSpeed/* * Time.deltaTime*/;
        //_image.transform.rotation = Quaternion.Lerp(_image.transform.rotation, Quaternion.Euler(rot), 0.7f);
    }

    private IEnumerator EndDrag(PointerEventData eventData)
    {
        while(_currentSpeed > 0)
        {
            Movement(0);
            Vector3 rot = _image.transform.eulerAngles;
            rot.z += -1 * _moveDirection.x * _currentSpeed/* * Time.deltaTime*/;
            _image.transform.rotation = Quaternion.Lerp(_image.transform.rotation, Quaternion.Euler(rot), 0.7f);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isPointerIn = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isPointerIn = false;
        StartCoroutine(EndDrag(eventData));
    }

    private void Start()
    {
        _dial = GetComponentInParent<Dial>();
        _selectCardList = new List<TestCard>();
        _image = GetComponent<Image>();
        //_image.alphaHitTestMinimumThreshold = 0.04f;
    }

    private void Update()
    {
        Swipe1();
    }

    public void Movement(int dir)
    {
        if(dir != 0)
        {
            if(dir != _moveDirection.x)
            {
                _currentSpeed = 0f;
            }
            _moveDirection.x = dir;
        }
        _currentSpeed = CulculateSpeed(dir);
    }

    private float CulculateSpeed(int dir)
    {
        if (dir != 0)
        {
            //_currentSpeed += _rotAcc * Time.deltaTime;
            _currentSpeed = _rotDamp * Time.deltaTime;
        }
        else
        {
            _currentSpeed -= _rotDeAcc * Time.deltaTime;
        }

        return Mathf.Clamp(_currentSpeed, 0, _rotDamp);
    }

    public void Swipe1()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchBeganPos = touch.position;
            }
            if (touch.phase == TouchPhase.Moved)
            {
                //if (_fingerID != -1 && _dial.GetSelectAreaForElement() == this)
                //{
                //    if (Input.touchCount > 0)
                //    {
                //        if (touch.deltaPosition.x < 0)
                //        {
                //            Vector3 rot = _image.transform.eulerAngles;
                //            rot.z += _rotDamp;
                //            _image.transform.rotation = Quaternion.Euler(rot);
                //        }
                //        else if (touch.deltaPosition.x > 0)
                //        {
                //            Vector3 rot = _image.transform.eulerAngles;
                //            rot.z -= _rotDamp;
                //            _image.transform.rotation = Quaternion.Euler(rot);
                //        }
                //    }
                //}
            }
            if (touch.phase == TouchPhase.Ended)
            {
                touchEndedPos = touch.position;
                touchDif = (touchEndedPos - touchBeganPos);

                //ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½. ï¿½ï¿½Ä¡ï¿½ï¿½ xï¿½Ìµï¿½ï¿½Å¸ï¿½ï¿½ï¿½ yï¿½Ìµï¿½ï¿½Å¸ï¿½ï¿½ï¿½ ï¿½Î°ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ Å©ï¿½ï¿½
                if (Mathf.Abs(touchDif.y) > swipeSensitivity || Mathf.Abs(touchDif.x) > swipeSensitivity)
                {
                    if (touchDif.y > 0 && Mathf.Abs(touchDif.y) > Mathf.Abs(touchDif.x))
                    {
                        //Debug.Log("up");
                    }
                    else if (touchDif.y < 0 && Mathf.Abs(touchDif.y) > Mathf.Abs(touchDif.x))
                    {
                        //Debug.Log("down");
                    }
                    else if (touchDif.x > 0 && Mathf.Abs(touchDif.y) < Mathf.Abs(touchDif.x))
                    {
                        //Debug.Log("right");
                        //Vector3 rot = _dialImage.transform.eulerAngles;
                        //rot.z -= _rotDamp;
                        //_dialImage.transform.rotation = Quaternion.Euler(rot);
                    }
                    else if (touchDif.x < 0 && Mathf.Abs(touchDif.y) < Mathf.Abs(touchDif.x))
                    {
                        //Debug.Log("Left");
                        //Vector3 rot = _dialImage.transform.eulerAngles;
                        //rot.z += _rotDamp;
                        //_dialImage.transform.rotation = Quaternion.Euler(rot);
                    }
                }
                //ï¿½ï¿½Ä¡.
                else
                {
                    //Debug.Log("touch");
                }
            }
        }
    }

    public bool IsHaveCard(TestCard card)
    {
        return _selectCardList.Find(x => x == card) != null;
    }

    public void AddSelectCard(TestCard card)
    {
        if(card != null)
        {
            if (_selectCardList.Count < _selectCount)
            {
                card.SetActiveOutline(true);
                _selectCardList.Add(card);

                if (_selectCardList.Count == _selectCount)
                {
                    _isAllSelect = true;
                }
                else
                {
                    _isAllSelect = false;
                }
            }
        }
    }

    public void RemoveSelectCard(TestCard card)
    {
        if(card != null)
        {
            if(_selectCardList.Count > 0)
            {
                card.SetActiveOutline(false);
                _selectCardList.Remove(card);

                if (_selectCardList.Count == _selectCount)
                {
                    _isAllSelect = true;
                }
                else
                {
                    _isAllSelect = false;
                }
            }
        }
    }

    public bool IsAllSelect()
    {
        return _isAllSelect;
    }
}
