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

    // Progress
    private Image _leftLine;
    private Image _rightLine;
    private Color _activeLineColor;
    private Color _inactiveLineColor;

    private GameObject _firstOutline;
    private TextMeshProUGUI _firstText;
    private GameObject _secondOutline;
    private TextMeshProUGUI _secondText;
    private GameObject _bossOutline;
    private TextMeshProUGUI _bossText;
    private Color _activeTextColor;
    private Color _inactiveTextColor;


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

    public void MapDescChange(MapRuneUI ui)
    {
        _mapDescText.SetText(ui.GetComponent<Stage>().StageDesc);
        _mapDescIcon.sprite = ui.transform.Find("Icon").GetComponent<SpriteRenderer>().sprite;
    }

    private void ProgressInit()
    {
        Transform progress = Managers.Canvas.GetCanvas("UserInfoPanelCanvas").transform.Find("Progress");
        _leftLine = progress.Find("LeftLine").GetComponent<Image>();
        _rightLine = progress.Find("RightLine").GetComponent<Image>();
        _activeLineColor = _leftLine.color;
        _inactiveLineColor = _rightLine.color;

        _firstOutline = progress.Find("FirstOutline").gameObject;
        _firstText = progress.Find("FirstText").GetComponent<TextMeshProUGUI>();
        _secondOutline = progress.Find("SecondOutline").gameObject;
        _secondText = progress.Find("SecondText").GetComponent<TextMeshProUGUI>();
        _bossOutline = progress.Find("BossOutline").gameObject;
        _bossText = progress.Find("BossText").GetComponent<TextMeshProUGUI>();
        _activeTextColor = _firstText.color;
        _inactiveTextColor = _secondText.color;
    }

    public void FirstProgress()
    {
        if (_leftLine == null) ProgressInit();

        _leftLine.color = _activeLineColor;
        _rightLine.color = _inactiveLineColor;

        _firstOutline.SetActive(true);
        _secondOutline.SetActive(false);
        _bossOutline.SetActive(false);

        _firstText.color = _activeTextColor;
        _secondText.color = _inactiveTextColor;
        _bossText.color = _inactiveTextColor;
    }

    public void SecondProgress()
    {
        if (_leftLine == null) ProgressInit();

        _leftLine.color = _inactiveLineColor;
        _rightLine.color = _activeLineColor;

        _firstOutline.SetActive(false);
        _secondOutline.SetActive(true);
        _bossOutline.SetActive(false);

        _firstText.color = _inactiveTextColor;
        _secondText.color = _activeTextColor;
        _bossText.color = _inactiveTextColor;
    }

    public void BossProgress()
    {
        if (_leftLine == null) ProgressInit();

        _leftLine.color = _inactiveLineColor;
        _rightLine.color = _inactiveLineColor;

        _firstOutline.SetActive(false);
        _secondOutline.SetActive(false);
        _bossOutline.SetActive(true);

        _firstText.color = _inactiveTextColor;
        _secondText.color = _inactiveTextColor;
        _bossText.color = _activeTextColor;
    }
}