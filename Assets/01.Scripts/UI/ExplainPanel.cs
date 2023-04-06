using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExplainPanel : MonoBehaviour
{
    #region UI Parameter

    private TextMeshProUGUI _nameText;
    private Image _runeImage;
    private TextMeshProUGUI _coolTimeText;
    private TextMeshProUGUI _descText;

    #endregion

    private void Start()
    {
        Setting();
    }

    private void Setting()
    {
        _nameText = transform.Find("Skill_Name_Text").GetComponent<TextMeshProUGUI>();
        _runeImage = transform.Find("Explain_Skill_Icon").GetComponent<Image>();
        _coolTimeText = transform.Find("CoolTime_Icon/CoolTime_Text").GetComponent<TextMeshProUGUI>();
        _descText = transform.Find("Explain_Text").GetComponent<TextMeshProUGUI>();
    }

    public void SetUI(Rune rune)
    {
        if(_nameText == null || _runeImage == null || _coolTimeText == null || _descText)
        {
            Setting();
        }

        _nameText.SetText(rune.GetRune().Name);
        _runeImage.sprite = rune.GetRune().RuneImage;
        _coolTimeText.SetText(rune.GetRune().CoolTime.ToString());
        _descText.SetText(rune.GetRune().MainRune.CardDescription);
    }
}
