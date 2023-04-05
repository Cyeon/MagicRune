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

        UIManager.Instance.Bind<Image>("Stage Arrow", CanvasManager.Instance.GetCanvas("MapUI").gameObject);

        _arrowImage = UIManager.Instance.Get<Image>("Stage Arrow");
    }

    public override void Clear()
    {
        
    }
}
