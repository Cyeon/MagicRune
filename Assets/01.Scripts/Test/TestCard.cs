using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class TestCard : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    protected RuneSO _rune;

    [SerializeField]
    protected bool _isEquip = false;
    public RuneSO Rune => _rune;

    private int _coolTime;

    private TestCardCollector _collector;
    private RectTransform _rect;

    protected virtual void Start()
    {
        _collector = GetComponentInParent<TestCardCollector>();
        _rect = GetComponent<RectTransform>();
    }

    public void SetRune(RuneSO rune)
    {
        _rune = rune;

        if (_rune == null)
        {
            this.GetComponent<Image>().color = Color.black;
        }
        else
        {
            this.GetComponent<Image>().color = Color.white;
            this.GetComponent<Image>().sprite = rune.RuneImage;

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
        if (_isEquip == false)
        {
            _collector.CardSelect(this);

            transform.localScale = new Vector3(1.5f, 1.5f, 1);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_isEquip == false)
        {
            _collector.CardSelect(null);
            transform.localScale = Vector3.one;
        }
    }

    public abstract void UseMainEffect();
    public abstract void UseAssistEffect();
}
