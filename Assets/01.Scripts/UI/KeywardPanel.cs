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

    private Keyword _keyward;

    public void SetKeyword(Keyword keyward)
    {
        _keyward = keyward;

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (_keyward == null) return;

        _nameText.SetText(_keyward.KeywardName);

        if(_keyward.KeywardType == KeywordType.Noraml)
            _descText.SetText(_keyward.KeywardDescription);
        else
            _descText.SetText(Resources.Load("Prefabs/Status/Status_" + _keyward.KeywardStatus).GetComponent<Status>().information);
    }
}
