using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapScene : BaseScene
{
    private Image _arrowImage;
    public Image ArrowImage => _arrowImage;

    private UserInfoUI _userInfoUI;

    protected override void Init()
    {
        base.Init();

        Managers.UI.Bind<UserInfoUI>("Upper_Frame", Managers.Canvas.GetCanvas("UserInfoPanelCanvas").gameObject);
        _userInfoUI = Managers.UI.Get<UserInfoUI>("Upper_Frame");
        Managers.GetPlayer().userInfoUI = _userInfoUI;

        _userInfoUI.UpdateHealthText();
        _userInfoUI.UpdateGoldText();

        Managers.Gold.UpdateGoldAction -= _userInfoUI.UpdateGoldText;
        Managers.Gold.UpdateGoldAction += _userInfoUI.UpdateGoldText;

        SceneType = Define.Scene.MapScene;
        Managers.Map.MapInit();
    }

    public override void Clear()
    {

    }
}
