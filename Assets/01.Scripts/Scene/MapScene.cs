using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapScene : BaseScene
{
    private TextMeshProUGUI _goldPopupText = null;

    private Image _arrowImage;
    public Image ArrowImage => _arrowImage;

    private UserInfoUI _userInfoUI;

    [SerializeField]
    private List<AudioClip> _bgmList = new List<AudioClip>(); 

    protected override void Init()
    {
        base.Init();

        Managers.UI.Bind<Image>("Stage Arrow", Managers.Canvas.GetCanvas("MapUI").gameObject);
        _arrowImage = Managers.UI.Get<Image>("Stage Arrow");

        Managers.UI.Bind<UserInfoUI>("Upper_Frame", Managers.Canvas.GetCanvas("UserInfoPanelCanvas").gameObject);
        _userInfoUI = Managers.UI.Get<UserInfoUI>("Upper_Frame");
        Managers.GetPlayer().userInfoUI = _userInfoUI;

        Managers.UI.Bind<TextMeshProUGUI>("GoldPopupText", Managers.Canvas.GetCanvas("UserInfoPanelCanvas").gameObject);
        _goldPopupText = Managers.UI.Get<TextMeshProUGUI>("GoldPopupText");

        _userInfoUI.UpdateHealthText();
        _userInfoUI.UpdateGoldText();

        SceneType = Define.Scene.MapScene;
        Managers.Map.MapInit();

        Managers.Sound.PlaySound(_bgmList[Managers.Map.Chapter - 1], SoundType.Bgm, true);

        Managers.GetPlayer().SetUIActive(false);
    }

    public override void Clear()
    {

    }

    private void GoldPopupDown()
    {
        _goldPopupText.SetText("");
    }

    public void GoldPopUp(int amount)
    {
        int flag = amount >= 0 ? 1 : -1;
        string str = flag == 1 ? "+ " : "- ";
        str += Mathf.Abs(amount).ToString();
        _goldPopupText.SetText(str);

        _goldPopupText.GetComponent<RectTransform>().DOAnchorPosY(50f * flag, 1f);
        Invoke("GoldPopupDown", 1.2f);
    }
}
