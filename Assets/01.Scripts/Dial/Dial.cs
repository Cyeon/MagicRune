using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Dial : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private List<Image> _dialImageList;
    [SerializeField]
    private float _rotDamp = 1;

    private int _fingerID = -1;

    [SerializeField]
    private GameObject tempCard;

    private Dictionary<int, List<TestCard>> _cardDict;
    private Dictionary<int, List<TestCard>> _selectCardDict;

    #region Swipt Parameta
    private Vector2 touchBeganPos;
    private Vector2 touchEndedPos;
    private Vector2 touchDif;
    [SerializeField]
    private float swipeSensitivity = 5;
    #endregion

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
        _cardDict = new Dictionary<int, List<TestCard>>(3);
        for (int i = 1; i <= 3; i++)
        {
            _cardDict.Add(i, new List<TestCard>());
        }
        _selectCardDict = new Dictionary<int, List<TestCard>>(3);
        for (int i = 1; i <= 3; i++)
        {
            _selectCardDict.Add(i, new List<TestCard>());
        }

        for (int i = 1; i <= 3; i++)
        {
            for(int j = 3; j <= 5; j++)
            {
                GameObject g = Instantiate(tempCard, this.transform.GetChild(i - 1));
                g.GetComponent<TestCard>().Dial = this;
                AddCard(g.GetComponent<TestCard>(), i);
            }
        }

        CardSort();
    }

    public void AddCard(TestCard card, int tier)
    {
        if (card != null)
        {
            _cardDict[tier].Add(card);

            CardSort();
        }
    }

    private void CardSort()
    {
        //float angle = -2 * Mathf.PI / _cardList.Count;

        //for (int i = 0; i < _cardList.Count; i++)
        //{
        //    float height = Mathf.Sin(angle * i + (90 * Mathf.Deg2Rad)) * _distance;
        //    float width = Mathf.Cos(angle * i + (90 * Mathf.Deg2Rad)) * _distance;
        //    _cardList[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(width, height, 0);

        //    Vector2 direction = new Vector2(
        //        _cardList[i].transform.position.x - transform.position.x,
        //        _cardList[i].transform.position.y - transform.position.y
        //    );

        //    float ang = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //    Quaternion angleAxis = Quaternion.AngleAxis(ang - 90f, Vector3.forward);
        //    //Quaternion rotation = Quaternion.Slerp(transform.rotation, angleAxis, rotateSpeed * Time.deltaTime);
        //    _cardList[i].GetComponent<RectTransform>().rotation = angleAxis;
        //}

        if (_cardDict.ContainsKey(1))
        {
            float angle = -2 * Mathf.PI / _cardDict[1].Count;

            for(int i = 0; i < _cardDict[1].Count; i++)
            {
                float height = Mathf.Sin(angle * i + (90 * Mathf.Deg2Rad)) * 600;
                float width = Mathf.Cos(angle * i + (90 * Mathf.Deg2Rad)) * 600;
                _cardDict[1][i].GetComponent<RectTransform>().anchoredPosition = new Vector3(width, height, 0);

                Vector2 direction = new Vector2(
                    _cardDict[1][i].transform.position.x - transform.position.x,
                    _cardDict[1][i].transform.position.y - transform.position.y
                );

                float ang = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion angleAxis = Quaternion.AngleAxis(ang - 90f, Vector3.forward);
                _cardDict[1][i].GetComponent<RectTransform>().rotation = angleAxis;
            }
        }

        if (_cardDict.ContainsKey(2))
        {
            float angle = -2 * Mathf.PI / _cardDict[1].Count;

            for (int i = 0; i < _cardDict[2].Count; i++)
            {
                float height = Mathf.Sin(angle * i + (90 * Mathf.Deg2Rad)) * 450;
                float width = Mathf.Cos(angle * i + (90 * Mathf.Deg2Rad)) * 450;
                _cardDict[2][i].GetComponent<RectTransform>().anchoredPosition = new Vector3(width, height, 0);

                Vector2 direction = new Vector2(
                    _cardDict[2][i].transform.position.x - transform.position.x,
                    _cardDict[2][i].transform.position.y - transform.position.y
                );

                float ang = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion angleAxis = Quaternion.AngleAxis(ang - 90f, Vector3.forward);
                _cardDict[2][i].GetComponent<RectTransform>().rotation = angleAxis;
            }
        }

        if (_cardDict.ContainsKey(3))
        {
            float angle = -2 * Mathf.PI / _cardDict[3].Count;

            for (int i = 0; i < _cardDict[3].Count; i++)
            {
                float height = Mathf.Sin(angle * i + (90 * Mathf.Deg2Rad)) * 250;
                float width = Mathf.Cos(angle * i + (90 * Mathf.Deg2Rad)) * 250;
                _cardDict[3][i].GetComponent<RectTransform>().anchoredPosition = new Vector3(width, height, 0);

                Vector2 direction = new Vector2(
                    _cardDict[3][i].transform.position.x - transform.position.x,
                    _cardDict[3][i].transform.position.y - transform.position.y
                );

                float ang = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion angleAxis = Quaternion.AngleAxis(ang - 90f, Vector3.forward);
                _cardDict[3][i].GetComponent<RectTransform>().rotation = angleAxis;
            }
        }
    }

    //public void Update()
    //{
    //    if(_fingerID != -1)
    //    {
    //        if(Input.touchCount > 0)
    //        {
    //            Touch t = Input.GetTouch(_fingerID);

    //            if (t.deltaPosition.x < 0)
    //            {
    //                Vector3 rot = _dialImage.transform.eulerAngles;
    //                rot.z += _rotDamp;
    //                _dialImage.transform.rotation = Quaternion.Euler(rot);
    //            }
    //            else if (t.deltaPosition.x > 0)
    //            {
    //                Vector3 rot = _dialImage.transform.eulerAngles;
    //                rot.z -= _rotDamp;
    //                _dialImage.transform.rotation = Quaternion.Euler(rot);
    //            }
    //        }
    //    }
    //}
}