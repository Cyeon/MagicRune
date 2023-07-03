using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RuneViewPanelUI : MonoBehaviour
{
    private Image _runeImage = null;
    private TextMeshProUGUI _runeNameText = null;
    private TextMeshProUGUI _runeDescText = null;
    private TextMeshProUGUI _runeCoolTimeText = null;
    private TextMeshProUGUI _grayRuneCoolTimeText = null;

    private TextMeshProUGUI _remainCoolTimeText = null;
    private GameObject _coolTImePanel = null;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        _runeImage = transform.Find("RuneImage").GetComponent<Image>();
        _runeNameText = transform.Find("RuneNameText").GetComponent<TextMeshProUGUI>();
        _runeDescText = transform.Find("RuneDescText").GetComponent<TextMeshProUGUI>();
        _runeCoolTimeText = transform.Find("RuneCoolTimeText").GetComponent<TextMeshProUGUI>();
        _remainCoolTimeText = transform.Find("RemainCoolTimeText").GetComponent<TextMeshProUGUI>();

        _coolTImePanel = transform.Find("CoolTimeMode").gameObject;
        _grayRuneCoolTimeText = _coolTImePanel.transform.Find("RuneCoolTimeText (1)").GetComponent<TextMeshProUGUI>();
    }

    public void SetUI(BaseRune baseRune, bool isCoolTime = false)
    {
        if (_runeImage == null || _runeNameText == null || _runeDescText == null || _runeCoolTimeText == null || _remainCoolTimeText == null || _grayRuneCoolTimeText == null)
        {
            Init();
        }

        _runeImage.sprite = baseRune.BaseRuneSO.RuneSprite;
        if (baseRune.IsEnhanced == false)
        {
            _runeNameText.color = Color.white;
            _runeNameText.SetText(baseRune.BaseRuneSO.RuneName);
        }
        else
        {
            _runeNameText.color = Color.green;
            _runeNameText.SetText(baseRune.BaseRuneSO.RuneName + "+");
        }
        _runeDescText.SetText(baseRune.BaseRuneSO.RuneDescription(baseRune.KeywordList, baseRune.IsEnhanced));
        _runeCoolTimeText.SetText(baseRune.BaseRuneSO.CoolTime.ToString());
        _grayRuneCoolTimeText.SetText(baseRune.BaseRuneSO.CoolTime.ToString());

        if (isCoolTime)
        {
            SetCoolTimeUI(baseRune);
            return;
        }

        _coolTImePanel.SetActive(false);
        _remainCoolTimeText.gameObject.SetActive(false);
    }

    private void SetCoolTimeUI(BaseRune baseRune)
    {
        _coolTImePanel.SetActive(true);
        _remainCoolTimeText.gameObject.SetActive(true);
        _remainCoolTimeText.SetText(baseRune.CoolTime.ToString());

    }
}