using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MapScene : BaseScene
{
    private TextMeshProUGUI _goldPopupText = null;

    private UserInfoUI _userInfoUI;

    [SerializeField]
    private List<AudioClip> _bgmList = new List<AudioClip>(); 
    public MapDial mapDial;

    [Header("Map Dial Panel")]
    [SerializeField] private Transform _mapDialPanel;
    private Image _compousProgress;

    private Image _mapDescIcon;
    private TextMeshProUGUI _mapDescText;

    [Header("Tutorial")]
    private Image _tutorialImage;

    protected override void Init()
    {
        base.Init();

        Managers.UI.Bind<UserInfoUI>("Upper_Frame", Managers.Canvas.GetCanvas("UserInfoPanelCanvas").gameObject);
        _userInfoUI = Managers.UI.Get<UserInfoUI>("Upper_Frame");
        Managers.GetPlayer().userInfoUI = _userInfoUI;

        Managers.UI.Bind<Image>("Progress", Managers.Canvas.GetCanvas("MapDial").gameObject);
        _compousProgress = Managers.UI.Get<Image>("Progress");

        if(_mapDialPanel == null)
        {
            _mapDialPanel = Managers.Canvas.GetCanvas("MapDial").transform.Find("MapDescPanel");
        }

        _mapDescIcon = _mapDialPanel.Find("IconSlot").Find("MapDescIcon").GetComponent<Image>();
        _mapDescText = _mapDialPanel.Find("Description").GetComponent<TextMeshProUGUI>();

        Managers.UI.Bind<TextMeshProUGUI>("GoldPopupText", Managers.Canvas.GetCanvas("UserInfoPanelCanvas").gameObject);
        _goldPopupText = Managers.UI.Get<TextMeshProUGUI>("GoldPopupText");

        _userInfoUI.UpdateHealthText();
        _userInfoUI.UpdateGoldText();

        Managers.Gold.UpdateGoldAction -= _userInfoUI.UpdateGoldText;
        Managers.Gold.UpdateGoldAction += _userInfoUI.UpdateGoldText;

        SceneType = Define.Scene.MapScene;
        Managers.Map.MapInit();

        Managers.Sound.PlaySound(_bgmList[Managers.Map.Chapter - 1], SoundType.Bgm, true);

        Managers.UI.Bind<Image>("TutorialImage", Managers.Canvas.GetCanvas("TutorialCanvas").gameObject);
        Managers.Canvas.GetCanvas("TutorialCanvas").enabled = false;
        _tutorialImage = Managers.UI.Get<Image>("TutorialImage");
    }

    private void Start()
    {
        mapDial.Clear();
        mapDial.MapStageSpawn();

        if(Managers.Map.isTutorial)
            Invoke("Tutorial", 1f);
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

    public void MapDescChange(MapRuneUI ui)
    {
        _mapDescText.SetText(ui.GetComponent<Stage>().StageDesc);
        _mapDescIcon.sprite = ui.transform.Find("Icon").GetComponent<SpriteRenderer>().sprite;
    }

    public void Tutorial()
    {
        Managers.Canvas.GetCanvas("TutorialCanvas").enabled = true;
        _tutorialImage.sprite = Resources.Load<Sprite>("Tutorial/MapDial");
        mapDial.DialElementList[0].IsDialLock = true;
    }
}
