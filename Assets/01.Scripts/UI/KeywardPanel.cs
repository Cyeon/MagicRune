using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeywardPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _nameText;
    [SerializeField]
    private TextMeshProUGUI _descText;

    private Keyward _keyward;

    public void SetKeyward(Keyward keyward)
    {
        _keyward = keyward;

        UpdateUI();
    }

    private void UpdateUI()
    {
        _nameText.SetText(_keyward.KeywardName);
        _descText.SetText(_keyward.KeywardDescription);
    }
}
