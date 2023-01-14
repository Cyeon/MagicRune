using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public abstract class Card : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    protected RuneSO _rune;

    [SerializeField]
    protected bool _isEquip = false;
    public RuneSO Rune => _rune;

    private CardCollector _collector;

    private int _coolTime;

    protected virtual void Start()
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

    public abstract void UseMainEffect();
    public abstract void UseAssistEffect();
}
