using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuneWeigetPanel : MonoBehaviour
{
    #region Dials
    private MapDial _mapDial = null;
    private RuneDial _runeDial = null;
    #endregion


    [SerializeField]
    private GameObject _parent;

    private List<RuneWeiget> _wegetList = new List<RuneWeiget>();

    [SerializeField]
    private AttributeSpecialization attributeSpecialization;

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
                attributeSpecialization.ChangeImage(_select.AttributeType);
            }
        }
    }

    void Start()
    {
        _mapDial = FindObjectOfType<MapDial>();
        _runeDial = FindObjectOfType<RuneDial>();

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

    public void DialLock()
    {
        _mapDial?.DialLock();
        _runeDial?.DialLock();
    }

    public void DialUnlock()
    {
        _mapDial?.DialUnlock();
        _runeDial?.DialUnlock();
    }
}
