using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicContent : MonoBehaviour
{
    [SerializeField]
    private List<CardSO> _cardList;
    public List<CardSO> CardList => _cardList;

    [SerializeField]
    private Vector2 _offset;
    [SerializeField]
    private GameObject cardTemplate;

    private const int CARD_SIZE_X = 300;

    public void AddCard(CardSO card)
    {
        _cardList.Add(card);

        GameObject go = Instantiate(cardTemplate, this.transform);
        go.transform.position = Vector3.zero;
        go.GetComponent<Card>().SetRune(card);
        go.GetComponent<Card>().IsEquipMagicCircle = true;

        Sort();
    }

    public void Clear()
    {
        for(int i = 0; i < _cardList.Count; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        _cardList.Clear();
    }

    public void Sort()
    {
        if (_cardList.Count == 0) return;

        float sideWidth = (1440 - CARD_SIZE_X * _cardList.Count) / 2;
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition3D = new Vector3(sideWidth + CARD_SIZE_X / 2 + CARD_SIZE_X * i + _offset.x, _offset.y, 0);

            if (i % 2 == 0)
            {
                transform.GetChild(i).rotation = Quaternion.Euler(0, 0, 15f);
            }
            else
            {
                transform.GetChild(i).rotation = Quaternion.Euler(0, 0, -15f);
            }
        }


    }
}
