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

    private RectTransform _rectTrm;

    private Keyward _keyward;

    public void SetKeyward(Keyward keyward)
    {
        _keyward = keyward;

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
        if (_keyward == null) return;

        _nameText.SetText(_keyward.KeywardName);

        if(_keyward.KeywardType == KeywardType.Noraml)
            _descText.SetText(_keyward.KeywardDescription);
        else
            _descText.SetText(Resources.Load("Prefabs/Status/Status_" + _keyward.KeywardStatus).GetComponent<Status>().information);
    }
}
