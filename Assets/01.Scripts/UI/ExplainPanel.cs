using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExplainPanel : MonoBehaviour
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

    private BaseRune _rune;

    private List<KeywardPanel> _keywardPanelList = new List<KeywardPanel>();

    public virtual void SetUI(BaseRune rune)
    {
        _rune = rune;

        _nameText.SetText(rune.BaseRuneSO.RuneName);
        _runeImage.sprite = rune.BaseRuneSO.RuneSprite;
        _coolTimeText.SetText(rune.BaseRuneSO.CoolTime.ToString());
        _descText.SetText(rune.BaseRuneSO.RuneDescription);

        ClearKeyward();

        SetKeyward();
    }

    private void ClearKeyward()
    {
        for(int i = 0; i < _keywardPanelList.Count; i++)
        {
            Managers.Resource.Destroy(_keywardPanelList[i].gameObject);
        }
    }

    private void SetKeyward()
    {
        for(int i = 0; i < _rune.BaseRuneSO.KeywardList.Length; i++)
        {
            KeywardPanel panel = Managers.Resource.Instantiate("UI/KeywardPanel", _keywardArea).GetComponent<KeywardPanel>();
            panel.SetKeyward(Managers.Keyward.GetKeyward(_rune.BaseRuneSO.KeywardList[i]));
        }
    }
}
