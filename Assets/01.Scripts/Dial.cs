using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Dial : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private Image _dialImage;
    [SerializeField]
    private float _rotDamp = 1;

    private int _fingerID = -1;

    [SerializeField]
    private float _distance;
    private List<Card> _cardList;
    [SerializeField]
    private GameObject tempCard;

    private Vector2 _startPos;
    private Vector2 _endPos;

    public void OnPointerDown(PointerEventData eventData)
    {
        _fingerID = eventData.pointerId;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _fingerID = -1;
    }

    private void Start()
    {
        //_cardList = new List<Card>();

        //for(int i = 0; i < 5; i++)
        //{
        //    GameObject g = Instantiate(tempCard, _dialImage.transform);
        //    AddCard(g.GetComponent<Card>());
        //}
    }

    public void AddCard(Card card)
    {
        if (card != null)
        {
            _cardList.Add(card);

            CardSort();
        }
    }

    private void CardSort()
    {
        float angle = -2 * Mathf.PI / _cardList.Count;

        for (int i = 0; i < _cardList.Count; i++)
        {
            float height = Mathf.Sin(angle * i + (90 * Mathf.Deg2Rad)) * _distance;
            float width = Mathf.Cos(angle * i + (90 * Mathf.Deg2Rad)) * _distance;
            _cardList[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(width, height, 0);

            Vector2 direction = new Vector2(
                _cardList[i].transform.position.x - transform.position.x,
                _cardList[i].transform.position.y - transform.position.y
            );

            float ang = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion angleAxis = Quaternion.AngleAxis(ang - 90f, Vector3.forward);
            //Quaternion rotation = Quaternion.Slerp(transform.rotation, angleAxis, rotateSpeed * Time.deltaTime);
            _cardList[i].GetComponent<RectTransform>().rotation = angleAxis;
        }
    }

    public void Update()
    {
        if(_fingerID != -1)
        {
            if(Input.touchCount > 0)
            {
                Touch t = Input.GetTouch(_fingerID);

                if (t.deltaPosition.x < 0)
                {
                    Vector3 rot = _dialImage.transform.eulerAngles;
                    rot.z += _rotDamp;
                    _dialImage.transform.rotation = Quaternion.Euler(rot);
                }
                else if (t.deltaPosition.x > 0)
                {
                    Vector3 rot = _dialImage.transform.eulerAngles;
                    rot.z -= _rotDamp;
                    _dialImage.transform.rotation = Quaternion.Euler(rot);
                }
            }
        }
    }
}
