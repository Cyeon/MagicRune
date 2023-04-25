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

        SceneType = Define.Scene.MapScene;

        Managers.Map.MapInit();

        Managers.UI.Bind<Image>("Stage Arrow", Managers.Canvas.GetCanvas("MapUI").gameObject);

        Managers.UI.Bind<UserInfoUI>("Upper_Frame", Managers.Canvas.GetCanvas("UserInfoPanelCanvas").gameObject, UIType.DontDestroyUI);
        _userInfoUI = Managers.UI.Get<UserInfoUI>("Upper_Frame", UIType.DontDestroyUI);
        Managers.Gold.UpdateGoldAction -= _userInfoUI.UpdateGoldText;
        Managers.Gold.UpdateGoldAction += _userInfoUI.UpdateGoldText;
        _arrowImage = Managers.UI.Get<Image>("Stage Arrow");
        Managers.Map.NextStage();
    }

    public override void Clear()
    {

    }
}
