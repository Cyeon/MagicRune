using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    public MapType type;
    public Image icon;

    private void Awake()
    {
        icon = GetComponent<Image>();
    }
}
