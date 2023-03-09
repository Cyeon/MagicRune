using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class DialElement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Dial _dial;
    private Image _image;

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
    private float _rotDamp = 3;
    [SerializeField]
    private float _touchDamp = 5f;
    private Vector3 _touchPos, offset;
    #endregion

    private int _fingerID = -1;

    private List<TestCard> _cardList;
    private TestCard _selectCard;
    public TestCard SelectCard
    {
        get => _selectCard;
        set
        {
            if(value == null)
            {
                if(_selectCard != null)
                {
                    _selectCard.SetActiveOutline(false);
                }
                _selectCard = value;
            }
            else
            {
                if(_selectCard != null)
                {
                    _selectCard.SetActiveOutline(false);
                }
                _selectCard = value;
                _selectCard.SetActiveOutline(true);
            }
        }
    }
    [SerializeField, Range(0f, 90f)]
    private float _selectOffset;
    private bool _isRotate = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        _fingerID = eventData.pointerId;
        _isRotate = true;

        _touchPos = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _fingerID = -1;
        _isRotate = false;

        float oneDinstance = 360f / _cardList.Count;
        bool inBoolean = (_image.transform.eulerAngles.z % oneDinstance) <= _selectOffset;
        bool outBoolean = (oneDinstance - (_image.transform.eulerAngles.z % oneDinstance)) <= _selectOffset;
        if (inBoolean)
        {
            // µ¹¸®°í ÀÖÀ» ¶§¸¸
            DOTween.To(() => _image.transform.eulerAngles, x => _image.transform.eulerAngles = x, new Vector3(0, 0, ((int)(_image.transform.eulerAngles.z / oneDinstance)) * oneDinstance), 0.3f);
        }
        else if (outBoolean)
        {
            DOTween.To(() => _image.transform.eulerAngles, x => _image.transform.eulerAngles = x, new Vector3(0, 0, ((int)(_image.transform.eulerAngles.z / oneDinstance) + 1) * oneDinstance), 0.3f);
        }
        //_dial.EditSelectArea(this);
    }

    private void Start()
    {
        _dial = GetComponentInParent<Dial>();
        _image = GetComponent<Image>();
        _cardList = new List<TestCard>();
        _image.alphaHitTestMinimumThreshold = 0.04f;
    }

    private void Update()
    {
        Swipe1();

        float oneDinstance = 360f / _cardList.Count;
        bool inBoolean = (_image.transform.eulerAngles.z % oneDinstance) <= _selectOffset;
        bool outBoolean = (oneDinstance - (_image.transform.eulerAngles.z % oneDinstance)) <= _selectOffset;
        if (inBoolean)
        {
            int index = (int)(_image.transform.eulerAngles.z / oneDinstance) % (_cardList.Count);
            SelectCard = _cardList[index];
            if (_isRotate == true)
            {
                UIManager.Instance.CardDescPopup(SelectCard);
            }
        }
        else if (outBoolean)
        {
            //int index = (int)(oneDinstance - (_image.transform.eulerAngles.z / oneDinstance)) % (_cardList.Count);
            //Debug.Log(index);
            //index = (index - 1) % _cardList.Count;
            //SelectCard = _cardList[index];

            int index = (int)(_image.transform.eulerAngles.z / oneDinstance) % (_cardList.Count);
            index = (index + 1) % _cardList.Count;
            SelectCard = _cardList[index];
            if (_isRotate == true)
            {
                UIManager.Instance.CardDescPopup(SelectCard);
            }
        }
        else
        {
            SelectCard = null;
            if (_isRotate == true)
            {
                UIManager.Instance.CardDescPopup(SelectCard);
            }
        }

        if (_isRotate)
        {
            offset = ((Vector3)Input.GetTouch(_fingerID).position - _touchPos);

            Vector3 rot = transform.eulerAngles;

            float temp = Input.GetTouch(_fingerID).position.x > Screen.width / 2 ? offset.x - offset.y : offset.x + offset.y;

            if (offset.x > 0)
                temp = Mathf.Clamp(temp, 0, offset.x);
            else
                temp = Mathf.Clamp(temp, offset.x, 0);

            rot.z += -1 * temp / _rotDamp;

            transform.rotation = Quaternion.Euler(rot);
            _touchPos = Input.GetTouch(_fingerID).position;
        }
    }

    public void SetCardList(List<TestCard> list)
    {
        _cardList = new List<TestCard>(list);
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
                    }
                    else if (touchDif.x < 0 && Mathf.Abs(touchDif.y) < Mathf.Abs(touchDif.x))
                    {
                        //Debug.Log("Left");
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
}
