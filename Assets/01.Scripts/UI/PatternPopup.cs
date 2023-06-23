using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PatternPopup : MonoBehaviour
{
    bool _isPoup = false;
    [SerializeField] private GameObject _patternPopupObj;

    private TextMeshPro _patternNameText;
    private TextMeshPro _patternDescText;

    private void Awake()
    {
        _patternNameText = _patternPopupObj.transform.Find("NameText").GetComponent<TextMeshPro>();
        _patternDescText = _patternPopupObj.transform.Find("DescText").GetComponent<TextMeshPro>();
    }

    public void Popup()
    {
        _isPoup = !_isPoup;
        _patternPopupObj.SetActive(_isPoup);

        if(_isPoup)
        {
            Pattern pattern = transform.parent.parent.GetComponent<Enemy>().PatternManager.CurrentPattern;
            _patternNameText.text = pattern.name;
            _patternDescText.text = pattern.PatternDescription;
        }
    }
}
