using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialTwoElement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private DialTwo _dial;

    private float _rotDamp = 3;
    [SerializeField]
    private float _touchDamp = 5f;
    private Vector3 _touchPos, _offset;

    private int _fingerID;

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
    }

    private void Start()
    {
        _dial = GetComponentInParent<DialTwo>();
    }

    private void Update()
    {
        if (_isRotate)
        {
            _offset = ((Vector3)Input.GetTouch(_fingerID).position - _touchPos);

            Vector3 rot = transform.eulerAngles;

            float temp = Input.mousePosition.x > Screen.width / 2 ? _offset.x - _offset.y : _offset.x + _offset.y;

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
            //_dial.RotateValue = rot.z;
            _touchPos = Input.GetTouch(_fingerID).position;
        }
    }
}
