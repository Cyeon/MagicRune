using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private RuneSO _rune;
    public RuneSO Rune => _rune;

    private CardCollector _collector;

    private void Start()
    {
        _collector = GetComponentInParent<CardCollector>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _collector.CardSelect(this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _collector.CardSelect(null);
    }
}
