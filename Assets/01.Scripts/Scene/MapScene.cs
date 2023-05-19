using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapScene : BaseScene
{
    private UserInfoUI _userInfoUI;

    public MapDial mapDial;

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

        mapDial = FindObjectOfType<MapDial>();

        SceneType = Define.Scene.MapScene;
        Managers.Map.MapInit();
    }

    private void Start()
    {

        mapDial.Clear();
        mapDial.MapStageSpawn();
    }

    public override void Clear()
    {

    }
}
