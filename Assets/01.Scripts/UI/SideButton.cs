using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SideButton : MonoBehaviour
{
    private Image _image;

    void Start()
    {
        _image = GetComponent<Image>();

        _image.alphaHitTestMinimumThreshold = 0.1f;
    }
}
