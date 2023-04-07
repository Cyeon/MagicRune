using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapScene : BaseScene
{
    private Image _arrowImage;
    public Image ArrowImage => _arrowImage;

    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.MapScene;

        Managers.UI.Bind<Image>("Stage Arrow", Managers.Canvas.GetCanvas("MapUI").gameObject);

        _arrowImage = Managers.UI.Get<Image>("Stage Arrow");
    }

    public override void Clear()
    {
        
    }
}
