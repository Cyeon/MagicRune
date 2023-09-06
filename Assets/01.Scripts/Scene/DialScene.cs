using DG.Tweening;
using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;

public class DialScene : BaseScene
{
    // Dial
    [SerializeField]
    private RuneDial _dial;
    public RuneDial Dial => _dial;

    // Status
    [SerializeField]
    private GameObject _DescriptionPanel;
    private TextMeshProUGUI _descNameText;
    private TextMeshProUGUI _descText;
    private RectTransform _statusDescPanelRectTrm;

    // Reward
    [SerializeField]
    private RewardUI _rewardUI;
    public RewardUI RewardUI => _rewardUI;
    public ChooseRuneUI ChooseRuneUI;

    // ETC
    [SerializeField]
    private KeywardRunePanel _cardDescPanel = null;
    private UserInfoUI _userInfoUI;
    private TextMeshProUGUI _goldPopupText = null;

    private void Start()
    {
        if(Managers.Map.currentStage.StageType == StageType.Boss)
        {
            BossAppearanceTimeline bossCutscene = Managers.Resource.Instantiate("Cutscene/BossDirector").GetComponent<BossAppearanceTimeline>();
            bossCutscene.SetEnemyInfo((int)Managers.Enemy.CurrentEnemy.enemyType);
            bossCutscene.Director.Play();
        }
    }

    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.DialScene;

        #region UI Bind
        Managers.UI.Bind<Image>("TurnBackground", Managers.Canvas.GetCanvas("Popup").gameObject);
        Managers.UI.Bind<TextMeshProUGUI>("TurnText", Managers.Canvas.GetCanvas("Popup").gameObject);

        Managers.UI.Bind<Image>("P StatusPanel", Managers.Canvas.GetCanvas("Main").gameObject);
        Managers.UI.Bind<Image>("E StatusPanel", Managers.Canvas.GetCanvas("Main").gameObject);

        Managers.UI.Bind<Button>("Restart Btn", Managers.Canvas.GetCanvas("Popup").gameObject);
        Managers.UI.Bind<Button>("Quit Btn", Managers.Canvas.GetCanvas("Popup").gameObject);

        Managers.UI.Bind<TextMeshProUGUI>("NameText", Managers.Canvas.GetCanvas("Popup").gameObject);
        Managers.UI.Bind<TextMeshProUGUI>("DescText", Managers.Canvas.GetCanvas("Popup").gameObject);

        Managers.UI.Bind<ChooseRuneUI>("ChooseRuneUI", Managers.Canvas.GetCanvas("Popup").gameObject);

        Managers.UI.Bind<UserInfoUI>("Upper_Frame", Managers.Canvas.GetCanvas("UserInfoPanelCanvas").gameObject);

        Managers.UI.Bind<TextMeshProUGUI>("GoldPopupText", Managers.Canvas.GetCanvas("UserInfoPanelCanvas").gameObject);

        
        #endregion

        _statusDescPanelRectTrm = _DescriptionPanel.transform.Find("Panel").GetComponent<RectTransform>();
        Managers.UI.TryGet<TextMeshProUGUI>("NameText", out _descNameText);
        Managers.UI.TryGet<TextMeshProUGUI>("DescText", out _descText);

        ChooseRuneUI = Managers.UI.Get<ChooseRuneUI>("ChooseRuneUI").GetComponent<ChooseRuneUI>();

        //UIManager.Instance.Get<Button>("Restart Btn").onClick.RemoveAllListeners();
        //UIManager.Instance.Get<Button>("Restart Btn").onClick.AddListener(() =>
        //{
        //    SceneManagerEX.Instance.LoadScene(Define.Scene.MapScene);
        //    //MapManager.Instance.ResetChapter();
        //}); 

        Managers.UI.Get<Button>("Quit Btn").onClick.RemoveAllListeners();
        Managers.UI.Get<Button>("Quit Btn").onClick.AddListener(() => Managers.GameQuit());

        Managers.Sound.PlaySound("BGM/DialSceneBGM", SoundType.Bgm, true, 1.0f);

        _userInfoUI = Managers.UI.Get<UserInfoUI>("Upper_Frame");

        _userInfoUI.UpdateHealthText();
        _userInfoUI.UpdateGoldText();

        Managers.Gold.UpdateGoldAction -= _userInfoUI.UpdateGoldText;
        Managers.Gold.UpdateGoldAction += _userInfoUI.UpdateGoldText;

        _goldPopupText = Managers.UI.Get<TextMeshProUGUI>("GoldPopupText");

        Managers.GetPlayer().userInfoUI = _userInfoUI;

        Managers.StatModifier.Init();
    }

    public void Turn(string text)
    {
        Image turnBG = Managers.UI.Get<Image>("TurnBackground");
        TextMeshProUGUI turnText = Managers.UI.Get<TextMeshProUGUI>("TurnText");

        turnBG.gameObject.SetActive(true);
        turnText.SetText(text);

        Sequence seq = DOTween.Sequence();
        seq.Append(turnBG.DOFade(0.5f, 0.5f));
        seq.Join(turnText.DOFade(1f, 0.5f));
        seq.AppendInterval(0.5f);
        seq.Append(turnBG.DOFade(0, 0.5f));
        seq.Join(turnText.DOFade(0, 0.5f));
        seq.AppendCallback(() => turnBG.gameObject.SetActive(false));
        seq.AppendCallback(() => BattleManager.Instance.TurnChange());
    }

    #region Status

    public StatusPanel GetStatusPanel(Status status, Transform parent, bool isPopup = false)
    {
        //StatusPanel statusPanel = Instantiate(isPopup ? _statusPopup : _statusPrefab).GetComponent<StatusPanel>();
        StatusPanel statusPanel = Managers.Resource.Instantiate("Status").GetComponent<StatusPanel>();
        statusPanel.Init(status);

        statusPanel.transform.SetParent(parent);
        statusPanel.transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);

        return statusPanel;
    }

    public StatusPanel GetPassivePanel(Passive passive, Transform parent, bool isPopup = false)
    {
        StatusPanel passivePanel = Managers.Resource.Instantiate("Status").GetComponent<StatusPanel>();
        passivePanel.Init(passive);

        passivePanel.transform.SetParent(parent);
        passivePanel.transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);

        return passivePanel;
    }

    private Transform GetStatusTrm(Unit unit)
    {
        return unit == BattleManager.Instance.Player ?
            Managers.UI.Get<Image>("P StatusPanel").transform : Managers.UI.Get<Image>("E StatusPanel").transform;
    }

    public void AddStatus(Unit unit, Status status)
    {
        GetStatusPanel(status, GetStatusTrm(unit));
    }

    public void AddPassive(Passive passive)
    {
        GetPassivePanel(passive, GetStatusTrm(Managers.Enemy.CurrentEnemy));
    }

    public void ClearStatusPanel(Unit unit)
    {
        Transform trm = GetStatusTrm(unit);
        for (int i = trm.childCount - 1; i >= 0; --i)
        {
            if (trm.GetChild(i).gameObject.name.Contains("패시브")) continue;
            Destroy(trm.GetChild(i).gameObject);
        }
    }


    public GameObject GetStatusPanelStatusObj(Unit unit, StatusName name)
    {
        Transform trm = GetStatusTrm(unit);

        GameObject obj = null;
        for (int i = 0; i < trm.childCount; i++)
        {
            if (trm.GetChild(i).GetComponent<StatusPanel>().Status?.statusName == name)
            {
                obj = trm.GetChild(i).gameObject;
            }
        }

        return obj;
    }

    public void ReloadStatusPanel(Unit unit, Status status)
    {
        GameObject obj = GetStatusPanelStatusObj(unit, status.statusName);

        if (obj == null)
            return;

        if (status.TypeValue <= 0)
        {
            RemoveStatusPanel(unit, status.statusName);
            return;
        }

        obj.GetComponent<StatusPanel>().UpdateDurationText();
    }

    public void RemoveStatusPanel(Unit unit, StatusName name)
    {
        GameObject obj = GetStatusPanelStatusObj(unit, name);

        if (obj == null)
        {
            return;
        }

        if (_descNameText != null)
        {
            if (_descNameText.text == obj.GetComponent<StatusPanel>().Status.debugName)
            {
                CloseDescriptionPanel();
            }
        }

        Managers.Resource.Destroy(obj);
    }

    public void AddStatusEffect(Unit unit, Status status)
    {
        Image effect = Managers.Resource.Instantiate("UI/StatusEffect", Managers.Canvas.GetCanvas("Popup").transform).GetComponent<Image>();
        effect.transform.position = Define.MainCam.WorldToScreenPoint(unit.transform.position);
        effect.sprite = status.icon;

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.1f);
        seq.Append(effect?.transform.DOScale(4.5f, 0.5f));
        seq.Join(effect?.DOFade(0f, 0.5f));
        seq.AppendCallback(() => Managers.Resource.Destroy(effect?.gameObject));
    }

    #endregion

    #region Popup
    public void DamageUIPopup(float amount, Vector3 pos, Status status = null)
    {
        DamagePopup popup = Managers.Resource.Instantiate("DamagePopup", Managers.Canvas.GetCanvas("Popup").transform).GetComponent<DamagePopup>();
        popup.Setup(amount, pos, status);
    }

    public void StatusPopup(Status status)
    {
        GameObject obj = Managers.Resource.Instantiate("StatusPopup", BattleManager.Instance.Enemy.transform);
        Image img = obj.GetComponent<Image>();
        img.sprite = status.icon;
        obj.transform.localScale = Vector3.one * 8f;

        Sequence seq = DOTween.Sequence();
        seq.Append(obj.transform.DOScale(9f, 0.7f).SetEase(Ease.InQuart));
        seq.Join(img.DOFade(0, 0.7f).SetEase(Ease.InQuart));
        seq.AppendCallback(() =>
        {
            Managers.Resource.Destroy(obj);
        });

        GameObject textPopup = Managers.Resource.Instantiate("StatusTextPopup", BattleManager.Instance.Enemy.transform);
        TextMeshProUGUI text = textPopup.GetComponent<TextMeshProUGUI>();
        text.text = status.debugName;
        textPopup.transform.localPosition = new Vector3(0, 300, 0);

        Sequence seq1 = DOTween.Sequence();
        seq1.Append(textPopup.transform.DOLocalMoveY(400f, 0.5f));
        seq1.Join(text.DOColor(Color.red, 0.5f));
        seq1.AppendInterval(0.3f);
        seq1.Join(text.DOFade(0, 0.5f).SetEase(Ease.InQuart));
        seq1.AppendCallback(() =>
        {
            Managers.Resource.Destroy(textPopup);
        });
    }

    public void InfoMessagePopup(string message, Vector3 pos)
    {
        InfoMessage popup = Managers.Resource.Instantiate("InfoMessage", Managers.Canvas.GetCanvas("Popup").transform).GetComponent<InfoMessage>();
        pos.z = 0;
        pos.y += 1;
        popup.Setup(message, pos);
    }
    #endregion

    #region Description

    public void CardDescPopup(BaseRune rune)
    {
        if (rune == null) return;

        _cardDescPanel.SetRune(rune);
        _cardDescPanel.SetUI(rune.BaseRuneSO, rune != null ? rune.IsEnhanced : false);
        _cardDescPanel.KeywardSetting();
    }

    public void CardDescDown()
    {
        _cardDescPanel.SetUI(null);
    }

    public void DescriptionPopup(string name, string desc, Vector3 pos)
    {
        _DescriptionPanel.SetActive(true);
        _DescriptionPanel.transform.position = pos + new Vector3(0, 20, 0);
        _DescriptionPanel.transform.DOMoveZ(-1, 0);

        if (pos.x < Screen.width / 2 && _statusDescPanelRectTrm.localPosition.x < 0)
        {
            _statusDescPanelRectTrm.DOLocalMoveX(-_statusDescPanelRectTrm.localPosition.x, 0);
        }
        else if(pos.x >= Screen.width / 2 && _statusDescPanelRectTrm.localPosition.x > 0)
        {
            _statusDescPanelRectTrm.DOLocalMoveX(-_statusDescPanelRectTrm.localPosition.x, 0);
        }

        if(_descNameText != null)
        {
            _descNameText.SetText(name);
        }
        if(_descText != null)
        {
            _descText.SetText(desc);
        }
    }

    public void CloseDescriptionPanel()
    {
        _DescriptionPanel.SetActive(false);
    }

    #endregion

    public override void Clear()
    {
        // ???モ닪筌?????룸챷留?????렊???モ닪筌??

        // ??袁⑥쓢?????????袁⑤?獄?
        // ?잙??????瑜곥돡????怨룸쭑??????
        for(int i = 0; i < _dial.DialElementList.Count; i++)
        {
            if(_dial.DialElementList[i] != null)
            {
                (_dial.DialElementList[i] as RuneDialElement).EffectHandler.Clear();
            }
        }
    }

    public void ChooseRuneUISetUp()
    {
        ChooseRuneUI.gameObject.SetActive(true);
        ChooseRuneUI.SetUp();
    }

    public void HideChooseRuneUI()
    {
        ChooseRuneUI.gameObject.SetActive(false);
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