using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusPanel : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI duration;
    public StatusName statusName;

    private void OnEnable()
    {
        image = GetComponent<Image>();
        duration = GetComponentInChildren<TextMeshProUGUI>();
    }
}
