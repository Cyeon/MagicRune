using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuneWeigetPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject _parent;

    private List<RuneWeiget> _wegetList = new List<RuneWeiget>();

    private RuneWeiget _select = null;
    public RuneWeiget Select
    {
        get => _select;
        set
        {
            if(_select != null)
            {
                // 이전에 있던 애 강조 표시 없애기
                _select.IsEmphasis = false;
            }
            _select = value;
            if(_select != null)
            {
                // 현재 있는 애 강조 표시하기
                _select.IsEmphasis = true;
                Managers.Rune.SetSelectAttribute(_select.AttributeType);
            }
        }
    }

    void Start()
    {
        GetComponentsInChildren<RuneWeiget>(_wegetList);

        _select = null;
    }

    public void SetActive(bool value)
    {
        if(Managers.Scene.CurrentScene.SceneType != Define.Scene.DialScene)
        {
            _parent.gameObject.SetActive(value);
        }
        else
        {
            _parent.gameObject.SetActive(false);
        }
    }
}
