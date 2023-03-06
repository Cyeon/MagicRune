using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestCard : MonoBehaviour, IPointerClickHandler
{
    public Dial Dial;
    private DialElement _dialElement;

    private void Start()
    {
        _dialElement = GetComponentInParent<DialElement>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_dialElement.IsHaveCard(this) == false)
            _dialElement.AddSelectCard(this);
    }
}