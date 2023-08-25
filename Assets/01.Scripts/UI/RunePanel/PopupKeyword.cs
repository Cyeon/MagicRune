using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupKeyword : MonoBehaviour
{
    [SerializeField]
    private Transform _keywordArea = null;
    private List<KeywardPanel> _keywordPanelList = new List<KeywardPanel>();

    private bool _isPopUp = false;

    private void Update()
    {
        if (_isPopUp && Input.GetMouseButtonDown(0))
        {
            ClearKeyword();
        }
    }

    public void MoveKeywordArea(Transform transform)
    {
        _keywordArea.position = transform.position + Vector3.one ;
    }

    public void SetKeyword(BaseRuneSO rune)
    {
        if (_keywordArea == null) return;

        for (int i = 0; i < rune.KeywardList.Length; i++)
        {
            KeywardPanel panel = Managers.Resource.Instantiate("UI/KeywardPanel", _keywordArea).GetComponent<KeywardPanel>();
            panel.transform.localScale = Vector3.one;
            panel.transform.localPosition = Vector3.zero;
            panel.SetKeyword(Managers.Keyward.GetKeyword(rune.KeywardList[i]));
            _keywordPanelList.Add(panel);
        }
        _isPopUp = true;
    }

    public void ClearKeyword()
    {
        for (int i = 0; i < _keywordPanelList.Count; i++)
        {
            Managers.Resource.Destroy(_keywordPanelList[i].gameObject);
        }
        _keywordPanelList.Clear();
        _isPopUp = false;
    }
}