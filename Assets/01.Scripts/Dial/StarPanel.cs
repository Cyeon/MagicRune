using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StarPanel : MonoBehaviour, IPointerClickHandler
{
    private Image _image;

    [SerializeField]
    private Dial _dial;

    public void OnPointerClick(PointerEventData eventData)
    {
        //_dial.EditSelectArea((_dial.GetSelectAreaForInt() + 1) % 4);
    }

    void Start()
    {
        _image = GetComponent<Image>();
        _image.alphaHitTestMinimumThreshold = 0.1f;
    }
}
