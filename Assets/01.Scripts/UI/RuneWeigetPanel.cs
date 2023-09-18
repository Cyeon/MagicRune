using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            if (_userInfoUI == null) _userInfoUI = Managers.Canvas.GetCanvas("UserInfoPanel").GetComponentInChildren<UserInfoUI>();

            if (_select != null)
            {
                // 이전에 있던 애 강조 표시 없애기
                _select.IsSelect = false;

                // 만약 똑같은 특화 선택시, 속성톡화 선택해제
                if (_select == value)
                {
                    _userInfoUI.WeigetInit(null);
                    Managers.Rune.SetSelectAttribute(AttributeType.None);
                    _select = null;
                    return;
                }
            }

            _select = value;
            if (_select != null)
            {
                // 현재 있는 애 강조 표시하기
                _select.IsSelect = true;
                _userInfoUI.WeigetInit(_select.onAttribute);
                Managers.Rune.SetSelectAttribute(_select.AttributeType);
            }
        }
    }

    private UserInfoUI _userInfoUI;

    void Start()
    {
        GetComponentsInChildren<RuneWeiget>(_wegetList);
        Select = _wegetList.Where(x => x.AttributeType == Managers.Rune.GetSelectAttribute()).FirstOrDefault();
    }

    public void SetActive(bool value)
    {
        if (Managers.Scene.CurrentScene.SceneType != Define.Scene.DialScene)
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
        Define.DialScene?.Dial.DialLock();
        Define.MapScene?.mapDial.DialLock();
    }

    public void DialUnlock()
    {
        Define.DialScene?.Dial.DialUnlock();
        Define.MapScene?.mapDial.DialUnlock();
    }
}
