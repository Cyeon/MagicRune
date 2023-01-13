using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public abstract class Card : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private GameObject cardPrefab = null;
    public GameObject CardPrefab => cardPrefab;

    [SerializeField]
    private CardSO _rune;

    [SerializeField]
    private bool _isEquipMagicCircle = false;
    public CardSO Rune => _rune;

    private CardCollector _collector;
    private bool _isRest = false;
    public bool IsRest => _isRest;

    private int _coolTime;
    public int CoolTime
    {
        get
        {
            return _coolTime;
        }
        set
        {
            _coolTime = value;
            if (_coolTime <= 0)
            {
                _isRest = false;
            }
            else
            {
                _isRest = true;
            }
        }
    }

    protected virtual void Start()
    {
        _collector = GetComponentInParent<CardCollector>();
    }

    public void SetRune(CardSO rune)
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

    public void SetIsEquip(bool value)
    {
        _isEquipMagicCircle = value;
    }

    public void SetCoolTime(int coolTime)
    {
        _coolTime = coolTime;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_isEquipMagicCircle == false)
        {
            _collector.CardSelect(this);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_isEquipMagicCircle == false)
        {
            _collector.CardSelect(null);
        }
    }

    public abstract void UseEffect();
}
