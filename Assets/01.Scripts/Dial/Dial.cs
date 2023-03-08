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
    private DeckSO _deck;

    private int _fingerID = -1;

    [SerializeField]
    private GameObject tempCard;

    private Dictionary<int, List<TestCard>> _cardDict;
    private List<DialElement> _dialElementList;

    [Range(1, 3)]
    private int _selectArea = 1;

    #region Swipe Parameta
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

        // 그냥 3으로 해도되는데 그냥 이렇게 함
        for (int i = 1; i <= _deck.list.Count; i++)
        {
            for (int j = 1; j <= _deck.list[i - 1].list.Count; j++)
            {
                GameObject g = Instantiate(tempCard, this.transform.GetChild(i - 1));
                TestCard c = g.GetComponent<TestCard>();
                c.Dial = this;
                c.SetMagic(_deck.list[i - 1].list[j - 1]);
                c.UpdateUI();
                AddCard(c, 4 - i);
            }
        }

        for (int i = 0; i < 3; i++)
        {
            DialElement d = this.transform.GetChild(i).GetComponent<DialElement>();
            d.SetCardList(_cardDict[3 - i]);
            _dialElementList.Add(d);
        }
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
        if (_cardDict.ContainsKey(3))
        {
            float angle = -2 * Mathf.PI / _cardDict[3].Count;

            for (int i = 0; i < _cardDict[3].Count; i++)
            {
                float height = Mathf.Sin(angle * i + (90 * Mathf.Deg2Rad)) * 695; // 470
                float width = Mathf.Cos(angle * i + (90 * Mathf.Deg2Rad)) * 695; // 450
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
            float angle = -2 * Mathf.PI / _cardDict[2].Count;

            for (int i = 0; i < _cardDict[2].Count; i++)
            {
                float height = Mathf.Sin(angle * i + (90 * Mathf.Deg2Rad)) * 510; // 470
                float width = Mathf.Cos(angle * i + (90 * Mathf.Deg2Rad)) * 510; // 450
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
            float angle = -2 * Mathf.PI / _cardDict[1].Count;

            for (int i = 0; i < _cardDict[1].Count; i++)
            {
                float height = Mathf.Sin(angle * i + (90 * Mathf.Deg2Rad)) * 328; // 470
                float width = Mathf.Cos(angle * i + (90 * Mathf.Deg2Rad)) * 328; // 450
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
    }

    public void AllMagicActive(bool value)
    {
        if (_cardDict.ContainsKey(1))
        {
            for (int i = 0; i < _cardDict[1].Count; i++)
            {
                _cardDict[1][i].gameObject.SetActive(false);
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
    }

    public void Attack()
    {
        Dictionary<EffectType, List<EffectObjectPair>> effectDict = new Dictionary<EffectType, List<EffectObjectPair>>();
        foreach (DialElement d in _dialElementList)
        {
            foreach(Pair p in d.SelectCard.Magic.MainRune.EffectDescription)
            {
                if (effectDict.ContainsKey(p.EffectType))
                {
                    effectDict[p.EffectType].Add(new EffectObjectPair(p, d.SelectCard.Magic.RuneEffect));
                }
                else
                {
                    effectDict.Add(p.EffectType, new List<EffectObjectPair> { new EffectObjectPair(p, d.SelectCard.Magic.RuneEffect) });
                }
            }
        }

        // 잘되내
        foreach(var d in effectDict)
        {
            Debug.Log($"{d.Key} : {d.Value}");
        }

        // 뭐... 히히 베지어 미사일 발싸!
    }
}