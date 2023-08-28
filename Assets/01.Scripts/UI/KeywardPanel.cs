using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class KeywardPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _nameText;
    [SerializeField]
    private TextMeshProUGUI _descText;

    private Keyword _keyword;
    private RectTransform _rectTrm;


    public void SetKeyword(Keyword keyward)
    {
        _keyword = keyward;

        UpdateUI();
    }

    public void SetWidth(float width = 356)
    {
        Vector2 size = _rectTrm.sizeDelta;
        size.x = width;
        _rectTrm.sizeDelta = size;
    }

    private void UpdateUI()
    {
        if (_keyword == null) return;

        _nameText.SetText(_keyword.KeywardName);

        if(_keyword.KeywardType == KeywordType.Normal)
            _descText.SetText(_keyword.KeywardDescription);
        else
            _descText.SetText(Resources.Load("Prefabs/Status/Status_" + _keyword.KeywardStatus).GetComponent<Status>().information);
    }
}
