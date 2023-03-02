using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AssistRune : MonoBehaviour
{
    private Transform _cardAreaParent;
    public Transform CardAreaParent => _cardAreaParent;
    private Transform _cardParent;
    private Image _cardBase;
    private Image _skillImage;
    private TMP_Text _costText;
    private Text _coolTimeText;
    private TMP_Text _mainSubText;
    private Text _skillText;
    private TMP_Text _nameText;
    private Text _assistRuneCount;
    private Image _descriptionImage;
    private TMP_Text _descText;
    private UIOutline _outlineEffect;

    void Awake()
    {
        Setting();
    }

    private void Setting()
    {
        _cardAreaParent = transform.Find("Card_Area");
        _cardBase = _cardAreaParent.Find("Base_Image/Card_Image").GetComponent<Image>();

        _nameText = _cardAreaParent.Find("Name_Text").GetComponent<TMP_Text>();
        _skillImage = _cardAreaParent.Find("Skill_Image").GetComponent<Image>();
        _costText = _cardAreaParent.Find("Cost_Text").GetComponent<TMP_Text>();
        _descText = _cardAreaParent.Find("Desc_Text").GetComponent<TMP_Text>();
    }

    public void UpdateUI(RuneProperty rune)
    {
        if (_cardAreaParent == null || _cardBase == null || _nameText == null || _skillImage == null || _costText == null || _descText == null)
            Setting();

        _nameText.SetText(rune.Name);
        _skillImage.sprite = rune.CardImage;
        _costText.SetText(rune.Cost.ToString());
        _descText.SetText(rune.CardDescription);
    }
}
