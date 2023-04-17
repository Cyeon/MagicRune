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

    public virtual void SetUI(BaseRune rune)
    {
        if (_nameText == null || _runeImage == null || _coolTimeText == null || _descText == null)
        {
            Setting();
        }

        _nameText.SetText(rune.BaseRuneSO.RuneName);
        _runeImage.sprite = rune.BaseRuneSO.RuneSprite;
        _coolTimeText.SetText(rune.BaseRuneSO.CoolTime.ToString());
        _descText.SetText(rune.BaseRuneSO.RuneDescription);
    }
}
