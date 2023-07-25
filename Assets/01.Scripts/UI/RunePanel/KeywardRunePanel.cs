using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class KeywardRunePanel : BasicRuneAddon
{
    [SerializeField]
    private Transform _keywardArea;

    private RectTransform _keywardRect;
    private List<KeywardPanel> _keywardPanelList = new List<KeywardPanel>();

    private void Start()
    {
        if (_keywardArea != null)
            _keywardRect = _keywardArea?.GetComponent<RectTransform>();
    }

    public override void SetUI(BaseRuneSO runeSO = null, bool isEnhance = true)
    {
        Basic.SetUI(runeSO, isEnhance);

        ClearKeyward();
    }

    public void KeywardSetting()
    {
        if(Basic.Rune != null)
            SetKeyward(Basic.Rune.BaseRuneSO);
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

        LayoutRebuilder.ForceRebuildLayoutImmediate(_keywardRect);
    }
}
