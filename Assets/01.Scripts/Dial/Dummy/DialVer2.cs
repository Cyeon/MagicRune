using DG.Tweening;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialVer2 : MonoBehaviour
{
    private Dial _dial;

    #region Swipe Parameta
    private Vector2 touchBeganPos;
    private Vector2 touchEndedPos;
    private Vector2 touchDif;
    [SerializeField]
    private float swipeSensitivity = 5;
    #endregion

    #region Drag Parameta
    [SerializeField]
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

    [SerializeField, Range(0f, 90f)]
    private float _selectOffset;
    private bool _isRotate = false;

    private bool _isCantRotate = false;

    private void Awake()
    {
        _dial = GetComponentInParent<Dial>();
        //_spriteRenderer.alphaHitTestMinimumThreshold = 0.04f;
    }

    private void Start()
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    private void Update()
    {
        if (_isCantRotate) return;
        Swipe1();

        RotateMagicCircle();
    }

    private void RotateMagicCircle()
    {
        if (_isRotate)
        {
            _offset = ((Vector3)Input.GetTouch(_fingerID).position - _touchPos);

            Vector3 rot = transform.eulerAngles;

            //float temp = Input.GetTouch(_fingerID).position.x > Screen.width / 2 ? _offset.x - _offset.y : _offset.x + _offset.y;

            float temp = _offset.x + _offset.y;

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

    public void SetLineID(int id)
    {
        _lineID = id;
    }

    public int GetLineID()
    {
        return _lineID;
    }

    public void Swipe1()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchBeganPos = touch.position;

                float distance = Vector2.Distance(Define.MainCam.ScreenToWorldPoint(touchBeganPos), (Vector2)this.transform.position);
                if (distance >= _inDistance &&
                    distance <= _outDistance)
                {
                    if (_isRotate == true) return;

                    _fingerID = touch.fingerId;
                    _isRotate = true;

                    _touchPos = touch.position;
                }
            }
            if (touch.phase == TouchPhase.Moved)
            {

            }
            if (touch.phase == TouchPhase.Ended)
            {
                if (_isRotate == true)
                {
                    _fingerID = -1;
                    _isRotate = false;
                    _isCantRotate = true;
                    Rewind();
                }

            }
        }

        
    }

    public void Rewind()
    {
        transform.DORotate(Vector3.zero, 0.1f).OnComplete(() =>
        {
            _isCantRotate = false;
        });
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
