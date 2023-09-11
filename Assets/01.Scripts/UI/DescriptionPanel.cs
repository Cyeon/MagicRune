using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DescriptionPanel : MonoBehaviour
{
    private TextMeshProUGUI _titleText;
    private TextMeshProUGUI _descriptionText;

    private void OnEnable()
    {
        _titleText = transform.Find("Title").GetComponent<TextMeshProUGUI>();
        _descriptionText = transform.Find("Description").GetComponent<TextMeshProUGUI>();
    }

    public void SetUp(string title, string desc)
    {
        _titleText.text = title;
        _descriptionText.text = desc;
    }
}
