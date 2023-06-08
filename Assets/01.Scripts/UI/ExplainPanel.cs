using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExplainPanel : MonoBehaviour, IPointerClickHandler
{
    #region UI Parameter

    [SerializeField]
    private TextMeshProUGUI _nameText;
    [SerializeField]
    private Image _runeImage;
    [SerializeField]
    private TextMeshProUGUI _coolTimeText;
    [SerializeField]
    private TextMeshProUGUI _descText;

    [SerializeField]
    private Transform _keywardArea;

    #endregion

    protected BaseRune _rune;
    public BaseRune Rune => _rune;

    private List<KeywardPanel> _keywardPanelList = new List<KeywardPanel>();

    private Action _action;

    public virtual void SetUI(BaseRune rune, bool isEnhance = false, bool isReward = true)
    {
        if (rune == null)
        {
            _rune = null;
            _nameText.SetText("");
            _runeImage.enabled = false;
            _coolTimeText.SetText("");
            _descText.SetText("");
            ClearKeyward();
            return;
        }

        _rune = rune;
        
        if(isEnhance == false)
        {
            _nameText.color = Color.white;
            _nameText.SetText(rune.BaseRuneSO.RuneName);
        }
        else
        {
            _nameText.color = Color.green;
            _nameText.SetText(_rune.BaseRuneSO.RuneName + "+");
        }
        _runeImage.enabled = true;
        _runeImage.sprite = rune.BaseRuneSO.RuneSprite;
        _coolTimeText.SetText(rune.BaseRuneSO.CoolTime.ToString());
        _descText.SetText(rune.BaseRuneSO.RuneDescription(isEnhance));

        ClearKeyward();
        if (isReward == true)
        {
            SetKeyward(rune.BaseRuneSO);
        }
    }

    public void SetAction(Action action)
    {
        _action = action;
    }

    public void SetUI(BaseRuneSO rune, bool isReward = true)
    {
        //_rune = rune;

        _nameText.SetText(rune.RuneName);
        _runeImage.enabled = true;
        _runeImage.sprite = rune.RuneSprite;
        _coolTimeText.SetText(rune.CoolTime.ToString());
        _descText.SetText(rune.RuneDescription());

        ClearKeyward();
        if (isReward == true)
        {
            SetKeyward(rune);
        }
    }

    public void ClearKeyward()
    {
        for (int i = 0; i < _keywardPanelList.Count; i++)
        {
            Managers.Resource.Destroy(_keywardPanelList[i].gameObject);
        }
        _keywardPanelList.Clear();
    }

    public void SetKeyward(BaseRuneSO rune)
    {
        if (_keywardArea == null) return;

        for (int i = 0; i < rune.KeywardList.Length; i++)
        {
            KeywardPanel panel = Managers.Resource.Instantiate("UI/KeywardPanel", _keywardArea).GetComponent<KeywardPanel>();
            panel.transform.localScale = Vector3.one;
            panel.SetKeyward(Managers.Keyward.GetKeyward(rune.KeywardList[i]));
            _keywardPanelList.Add(panel);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _action?.Invoke();
    }
}
