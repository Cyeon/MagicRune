using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialElement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Dial _dial;
    private Image _image;

    [SerializeField]
    private float _rotDamp = 3;
    [SerializeField]
    private int _selectCount = 3;

    #region Swipt Parameta
    private Vector2 touchBeganPos;
    private Vector2 touchEndedPos;
    private Vector2 touchDif;
    [SerializeField]
    private float swipeSensitivity = 5;
    #endregion

    private int _fingerID = -1;

    private bool _isSelect = false;
    public bool IsSelect { get => _isSelect; set => _isSelect = value; }
    private bool _isAllSelect = false;

    private List<TestCard> _selectCardList;

    public void OnPointerDown(PointerEventData eventData)
    {
        _fingerID = eventData.pointerId;

        _dial.EditSelectArea(this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _fingerID = -1;
    }

    private void Start()
    {
        _dial = GetComponentInParent<Dial>();
        _selectCardList = new List<TestCard>();
        _image = GetComponent<Image>();
        _image.alphaHitTestMinimumThreshold = 0.0f;
    }

    private void Update()
    {
        Swipe1();
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
                if (_fingerID != -1 && _dial.GetSelectAreaForElement() == this)
                {
                    if (Input.touchCount > 0)
                    {
                        if (touch.deltaPosition.x < 0)
                        {
                            Vector3 rot = _image.transform.eulerAngles;
                            rot.z += _rotDamp;
                            _image.transform.rotation = Quaternion.Euler(rot);
                        }
                        else if (touch.deltaPosition.x > 0)
                        {
                            Vector3 rot = _image.transform.eulerAngles;
                            rot.z -= _rotDamp;
                            _image.transform.rotation = Quaternion.Euler(rot);
                        }
                    }
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
                //��ġ.
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
            if(_selectCardList.Count < _selectCount)
            {
                card.SetActiveOutline(true);
                _selectCardList.Add(card);

                if(_selectCardList.Count == _selectCount)
                {
                    _isAllSelect = true;
                }
            }
        }
    }

    public bool IsAllSelect()
    {
        return _isAllSelect;
    }
}
