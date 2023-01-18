using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class MagicCircleBgPanel : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private MagicCircle _magicCircle;

    public void OnPointerClick(PointerEventData eventData)
    {
        _magicCircle.IsBig = false;
    }
}
