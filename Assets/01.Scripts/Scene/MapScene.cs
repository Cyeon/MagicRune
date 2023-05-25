using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapScene : BaseScene
{
    private TextMeshProUGUI _goldPopupText = null;

    private UserInfoUI _userInfoUI;

    [SerializeField]
    private List<AudioClip> _bgmList = new List<AudioClip>(); 
    public MapDial mapDial;

    private Image _compousProgress;

    protected override void Init()
    {
        base.Init();

        Managers.UI.Bind<UserInfoUI>("Upper_Frame", Managers.Canvas.GetCanvas("UserInfoPanelCanvas").gameObject);
        _userInfoUI = Managers.UI.Get<UserInfoUI>("Upper_Frame");
        Managers.GetPlayer().userInfoUI = _userInfoUI;

        Managers.UI.Bind<Image>("Progress", Managers.Canvas.GetCanvas("CompousProgressCanvas").gameObject);
        _compousProgress = Managers.UI.Get<Image>("Progress");

        Managers.UI.Bind<TextMeshProUGUI>("GoldPopupText", Managers.Canvas.GetCanvas("UserInfoPanelCanvas").gameObject);
        _goldPopupText = Managers.UI.Get<TextMeshProUGUI>("GoldPopupText");

        _userInfoUI.UpdateHealthText();
        _userInfoUI.UpdateGoldText();

        Managers.Gold.UpdateGoldAction -= _userInfoUI.UpdateGoldText;
        Managers.Gold.UpdateGoldAction += _userInfoUI.UpdateGoldText;

        SceneType = Define.Scene.MapScene;
        Managers.Map.MapInit();

        Managers.Sound.PlaySound(_bgmList[Managers.Map.Chapter - 1], SoundType.Bgm, true);

        Managers.GetPlayer().SetUIActive(false);
    }

    private void Start()
    {
        mapDial.Clear();
        mapDial.MapStageSpawn();
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

    public void CompousProgress(float amount)
    {
        _compousProgress.DOFillAmount(amount, 0.25f);
    }
}
