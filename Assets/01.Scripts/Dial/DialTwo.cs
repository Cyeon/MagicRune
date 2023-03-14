using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialTwo : MonoBehaviour
{
    [SerializeField]
    private DeckSO _deck;
    [SerializeField]
    private TestCard _tempCard;

    [SerializeField]
    private List<GameObject> _tierObjectList = new List<GameObject>();
    private List<TestCard> _cardList = new List<TestCard>();
    private List<TestCard> _selectCard = new List<TestCard>();

    private int _selectArea = 0;

    private float _rotateValue;
    public float RotateValue { get => _rotateValue; set => _rotateValue = value; }

    private void Start()
    {
        CreateCard(3);
    }

    private void CreateCard(int area = 1)
    {
        //if (_selectArea == area) return;
        foreach(var card in _cardList)
        {
            Destroy(card);
        }
        _cardList.Clear();

        _selectArea = area;

        foreach(var g in _tierObjectList)
        {
            g.SetActive(false);
        }

        switch (_selectArea)
        {
            case 1:
                //_tierObjectList[0].SetActive(true);
                break;
            case 2:
                _tierObjectList[0].SetActive(true);
                //_tierObjectList[1].SetActive(true);
                break;
            case 3:
                _tierObjectList[0].SetActive(true);
                _tierObjectList[1].SetActive(true);
                //_tierObjectList[2].SetActive(true);
                break;
        }

        float angle = -2 * Mathf.PI / _deck.List[_selectArea - 1].List.Count;
        for (int i = 0; i < _deck.List[_selectArea - 1].List.Count; i++)
        {
            TestCard c = Instantiate(_tempCard, this.transform.Find("Element"));

            float height = Mathf.Sin(angle * i + (90 * Mathf.Deg2Rad)) * 525; // 470
            float width = Mathf.Cos(angle * i + (90 * Mathf.Deg2Rad)) * 525; // 450
            c.GetComponent<RectTransform>().anchoredPosition = new Vector3(width, height, 0);

            Vector2 direction = new Vector2(
                c.transform.position.x - transform.position.x,
                c.transform.position.y - transform.position.y
            );

            float ang = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion angleAxis = Quaternion.AngleAxis(ang - 90f, Vector3.forward);
            c.GetComponent<RectTransform>().rotation = angleAxis;

            _cardList.Add(c);
        }
    }

    public void CardSort()
    {
        float angle = -2 * Mathf.PI / _cardList.Count;
        for (int i = 0; i < _cardList.Count; i++)
        {
            float height = Mathf.Sin(angle * i + (90 * Mathf.Deg2Rad)) * 525;
            float width = Mathf.Cos(angle * i + (90 * Mathf.Deg2Rad)) * 525;
            _cardList[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(width, height, 0);

            Vector2 direction = new Vector2(
                _cardList[i].transform.position.x - transform.position.x,
                _cardList[i].transform.position.y - transform.position.y
            );

            float ang = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion angleAxis = Quaternion.AngleAxis(ang - 90f, Vector3.forward);
            _cardList[i].GetComponent<RectTransform>().rotation = angleAxis;
        }
    }
}
