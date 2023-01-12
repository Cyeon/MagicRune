using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class Card : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private RuneSO _rune;

    [SerializeField]
    private bool _isEquip = false;
    [SerializeField]
    private float _distance = 350;
    public RuneSO Rune => _rune;

    private CardCollector _collector;

    private int _coolTime;

    private void Start()
    {
        _collector = GetComponentInParent<CardCollector>();
    }

    public void SetRune(RuneSO rune)
    {
        _rune = rune;

        if(_rune == null)
        {
            this.transform.GetChild(0).GetComponent<Image>().color = Color.black;
        }
        else
        {
            this.transform.GetChild(0).GetComponent<Image>().color = Color.red;
        }
    }

    public void CardAnimation()
    {
        this.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        this.transform.GetChild(0).GetComponent<RectTransform>().DOLocalMoveX(_distance, 0.3f);
    }

    public void SetIsEquip(bool value)
    {
        _isEquip = value;
    }

    public void SetCoolTime(int coolTime)
    {
        _coolTime = coolTime;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(_isEquip == false)
        {
            _collector.CardSelect(this);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_isEquip == false)
        {
            _collector.CardSelect(null);
        }
    }
}
