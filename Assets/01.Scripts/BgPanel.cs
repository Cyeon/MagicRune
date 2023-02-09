using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BgPanel : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private MagicCircle _magicCircle;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_magicCircle != null)
        {
            _magicCircle.CardCollector.AllCardDescription(false);
        }
    }
}
