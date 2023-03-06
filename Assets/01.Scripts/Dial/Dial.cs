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
    private List<DialElement> _dialElementList;

    [Range(1, 3)]
    private int _selectArea = 1;

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
        _dialElementList = new List<DialElement>();
        for(int i = 0; i < 3; i++)
        {
            _dialElementList.Add(this.transform.GetChild(i).GetComponent<DialElement>());
        }

        for (int i = 1; i <= 3; i++)
        {
            for(int j = 1; j <= 5; j++)
            {
                GameObject g = Instantiate(tempCard, this.transform.GetChild(i - 1));
                g.GetComponent<TestCard>().Dial = this;
                AddCard(g.GetComponent<TestCard>(), 4 - i);
            }
        }

        EditSelectArea(3);
        //CardSort();
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
        switch (_selectArea)
        {
            case 3:
                if (_cardDict.ContainsKey(3))
                {
                    float angle = -2 * Mathf.PI / _cardDict[3].Count;
                    float distance = (_dialElementList[0].GetComponent<RectTransform>().sizeDelta.x - _dialElementList[1].GetComponent<RectTransform>().sizeDelta.x / 2) / 2;
                    //distance += _dialElementList[1].GetComponent<RectTransform>().sizeDelta.x / 2;
                    distance += 50;

                    for (int i = 0; i < _cardDict[3].Count; i++)
                    {
                        float height = Mathf.Sin(angle * i + (90 * Mathf.Deg2Rad)) * distance;
                        float width = Mathf.Cos(angle * i + (90 * Mathf.Deg2Rad)) * distance;
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

                if (_cardDict.ContainsKey(2))
                {
                    for (int i = 0; i < _cardDict[2].Count; i++)
                    {
                        _cardDict[2][i].gameObject.SetActive(false);
                    }
                }
                if (_cardDict.ContainsKey(1))
                {
                    for (int i = 0; i < _cardDict[1].Count; i++)
                    {
                        _cardDict[1][i].gameObject.SetActive(false);
                    }
                }
                if (_cardDict.ContainsKey(3))
                {
                    for (int i = 0; i < _cardDict[3].Count; i++)
                    {
                        _cardDict[3][i].gameObject.SetActive(true);
                    }
                }
                break;
            case 2:
                if (_cardDict.ContainsKey(2))
                {
                    float angle = -2 * Mathf.PI / _cardDict[2].Count;
                    float distance = (_dialElementList[1].GetComponent<RectTransform>().sizeDelta.x - _dialElementList[2].GetComponent<RectTransform>().sizeDelta.x) / 2;
                    //distance += _dialElementList[2].GetComponent<RectTransform>().sizeDelta.x / 2;
                    distance += 50;

                    for (int i = 0; i < _cardDict[2].Count; i++)
                    {
                        float height = Mathf.Sin(angle * i + (90 * Mathf.Deg2Rad)) * distance;
                        float width = Mathf.Cos(angle * i + (90 * Mathf.Deg2Rad)) * distance;
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

                if (_cardDict.ContainsKey(1))
                {
                    for (int i = 0; i < _cardDict[1].Count; i++)
                    {
                        _cardDict[1][i].gameObject.SetActive(false);
                    }
                }
                if (_cardDict.ContainsKey(3))
                {
                    for (int i = 0; i < _cardDict[3].Count; i++)
                    {
                        _cardDict[3][i].gameObject.SetActive(false);
                    }
                }
                if (_cardDict.ContainsKey(2))
                {
                    for (int i = 0; i < _cardDict[2].Count; i++)
                    {
                        _cardDict[2][i].gameObject.SetActive(true);
                    }
                }
                break;
            case 1:
                if (_cardDict.ContainsKey(1))
                {
                    float angle = -2 * Mathf.PI / _cardDict[1].Count;
                    float distance = (_dialElementList[2].GetComponent<RectTransform>().sizeDelta.x - _dialElementList[2].GetComponent<RectTransform>().sizeDelta.x / 2) / 2;
                    //distance += _dialElementList[2].GetComponent<RectTransform>().sizeDelta.x / 2;
                    distance += 50;

                    for (int i = 0; i < _cardDict[1].Count; i++)
                    {
                        float height = Mathf.Sin(angle * i + (90 * Mathf.Deg2Rad)) * distance;
                        float width = Mathf.Cos(angle * i + (90 * Mathf.Deg2Rad)) * distance;
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
                    for (int i = 0; i < _cardDict[2].Count; i++)
                    {
                        _cardDict[2][i].gameObject.SetActive(false);
                    }
                }
                if (_cardDict.ContainsKey(3))
                {
                    for (int i = 0; i < _cardDict[3].Count; i++)
                    {
                        _cardDict[3][i].gameObject.SetActive(false);
                    }
                }
                if (_cardDict.ContainsKey(1))
                {
                    for (int i = 0; i < _cardDict[1].Count; i++)
                    {
                        _cardDict[1][i].gameObject.SetActive(true);
                    }
                }
                break;
            default:
                break;
        }
    }

    public void EditSelectArea(int index, float dotweenTime = 0.2f)
    {
        Sequence seq = DOTween.Sequence();
        switch (index)
        {
            case 1:
                // 안개 두개가 커짐
                seq.Append(_dialElementList[1].GetComponent<RectTransform>().DOSizeDelta(new Vector2(1150, 1150), dotweenTime));
                seq.Join(_dialElementList[2].GetComponent<RectTransform>().DOSizeDelta(new Vector2(900, 900), dotweenTime));
                break;
            case 2:
                // 중간이 커지고 가장 안에가 작아짐
                seq.Append(_dialElementList[1].GetComponent<RectTransform>().DOSizeDelta(new Vector2(1150, 1150), dotweenTime));
                seq.Join(_dialElementList[2].GetComponent<RectTransform>().DOSizeDelta(new Vector2(500, 500), dotweenTime));
                break;
            case 3:
                // 안에 두개가 작아짐
                seq.Append(_dialElementList[1].GetComponent<RectTransform>().DOSizeDelta(new Vector2(800, 800), dotweenTime));
                seq.Join(_dialElementList[2].GetComponent<RectTransform>().DOSizeDelta(new Vector2(500, 500), dotweenTime));
                break;
            default:
                break;
        }
        seq.AppendCallback(() =>
        {
            _selectArea = index;

            CardSort();
        });
    }

    public void EditSelectArea(DialElement dial)
    {
        int index = -1;
        for(int i = 0; i < _dialElementList.Count; i++)
        {
            if (_dialElementList[i] == dial)
            {
                index = i;
                break;
            }
        }

        if (index == -1) return;

        EditSelectArea(3 - index);
    }

    public int GetSelectAreaForInt()
    {
        return _selectArea;
    }

    public DialElement GetSelectAreaForElement()
    {
        return _dialElementList[3 - _selectArea];
    }
}