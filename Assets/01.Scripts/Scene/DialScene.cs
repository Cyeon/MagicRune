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
    [SerializeField]
    private RuneDial _dial;
    public RuneDial Dial => _dial;

    // ?곸궠?獄????닿뎄 ???븐꽢
    [SerializeField]
    private KeywardRunePanel _cardDescPanel = null;

    // ??⑤객臾??怨대쭜 ???닿뎄 ???븐꽢
    [SerializeField]
    private GameObject _statusDescPanel;
    private TextMeshProUGUI _statusDescName;
    private TextMeshProUGUI _statusDescInfo;
    private RectTransform _statusDescPanelRectTrm;

    // ?熬곥굥?????곌랜?삥묾?
    [SerializeField]
    private RewardUI _rewardUI;
    public RewardUI RewardUI => _rewardUI;
    private ChooseRuneUI _chooseRuneUI;

    // ??? ??⑤８堉딁뛾?
    private UserInfoUI _userInfoUI;

    private TextMeshProUGUI _goldPopupText = null;

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

        Managers.UI.Bind<TextMeshProUGUI>("Status_Name_Text", Managers.Canvas.GetCanvas("Popup").gameObject);
        Managers.UI.Bind<TextMeshProUGUI>("Status_Desc_Text", Managers.Canvas.GetCanvas("Popup").gameObject);

        Managers.UI.Bind<ChooseRuneUI>("ChooseRuneUI", Managers.Canvas.GetCanvas("Popup").gameObject);

        Managers.UI.Bind<UserInfoUI>("Upper_Frame", Managers.Canvas.GetCanvas("UserInfoPanelCanvas").gameObject);

        Managers.UI.Bind<TextMeshProUGUI>("GoldPopupText", Managers.Canvas.GetCanvas("UserInfoPanelCanvas").gameObject);
        #endregion

        _statusDescPanelRectTrm = _statusDescPanel.transform.Find("Panel").GetComponent<RectTransform>();
        _statusDescName = Managers.UI.Get<TextMeshProUGUI>("Status_Name_Text");
        _statusDescInfo = Managers.UI.Get<TextMeshProUGUI>("Status_Desc_Text");

        _chooseRuneUI = Managers.UI.Get<ChooseRuneUI>("ChooseRuneUI").GetComponent<ChooseRuneUI>();

        //UIManager.Instance.Get<Button>("Restart Btn").onClick.RemoveAllListeners();
        //UIManager.Instance.Get<Button>("Restart Btn").onClick.AddListener(() =>
        //{
        //    SceneManagerEX.Instance.LoadScene(Define.Scene.MapScene);
        //    //MapManager.Instance.ResetChapter();
        //});
        Managers.UI.Get<Button>("Quit Btn").onClick.RemoveAllListeners();
        Managers.UI.Get<Button>("Quit Btn").onClick.AddListener(() => Managers.GameQuit());

        Managers.Sound.PlaySound("BGM/DialSceneBGM", SoundType.Bgm, true, 1.0f);

        Debug.Log($"Resolution : {Screen.width}, {Screen.height}");
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

    private Transform GetStatusTrm(Unit unit)
    {
        return unit == BattleManager.Instance.Player ?
            Managers.UI.Get<Image>("P StatusPanel").transform : Managers.UI.Get<Image>("E StatusPanel").transform;
    }

    public void AddStatus(Unit unit, Status status)
    {
        GetStatusPanel(status, GetStatusTrm(unit));
    }

    public void ClearStatusPanel(Unit unit)
    {
        Transform trm = GetStatusTrm(unit);
        for (int i = trm.childCount - 1; i >= 0; --i)
        {
            Destroy(trm.GetChild(i).gameObject);
        }
    }


    public GameObject GetStatusPanelStatusObj(Unit unit, StatusName name)
    {
        Transform trm = GetStatusTrm(unit);

        GameObject obj = null;
        for (int i = 0; i < trm.childCount; i++)
        {
            if (trm.GetChild(i).GetComponent<StatusPanel>().StatusName == name)
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

        if(_statusDescName.text == obj.GetComponent<StatusPanel>().Status.debugName)
        {
            CloseStatusDescPanel();
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
        seq.Append(effect.transform.DOScale(4.5f, 0.5f));
        seq.Join(effect.DOFade(0f, 0.5f));
        seq.AppendCallback(() => Managers.Resource.Destroy(effect.gameObject));
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

        _cardDescPanel.SetUI(rune.BaseRuneSO, rune != null ? rune.IsEnhanced : false);
        _cardDescPanel.KeywardSetting();
    }

    public void CardDescDown()
    {
        _cardDescPanel.SetUI(null);
    }

    public void StatusDescPopup(Status status, Vector3 pos)
    {
        _statusDescPanel.SetActive(true);
        _statusDescPanel.transform.position = pos + new Vector3(0, 20, 0);
        _statusDescPanel.transform.DOMoveZ(-1, 0);

        if (pos.x < Screen.width / 2 && _statusDescPanelRectTrm.localPosition.x < 0)
        {
            _statusDescPanelRectTrm.DOLocalMoveX(-_statusDescPanelRectTrm.localPosition.x, 0);
        }
        else if(pos.x >= Screen.width / 2 && _statusDescPanelRectTrm.localPosition.x > 0)
        {
            _statusDescPanelRectTrm.DOLocalMoveX(-_statusDescPanelRectTrm.localPosition.x, 0);
        }

        _statusDescName.SetText(status.debugName);
        _statusDescInfo.SetText(status.information);
    }

    public void CloseStatusDescPanel()
    {
        _statusDescPanel.SetActive(false);
    }

    #endregion

    public override void Clear()
    {
        // ?醫롫짗????룸쐻??덉굲?醫롫짗??

        // ?댄럺???대━??肄붾뱶.
        // 洹쇰뜲 ?섏쨷???놁븷??????
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
        _chooseRuneUI.gameObject.SetActive(true);
        _chooseRuneUI.SetUp();
    }

    public void HideChooseRuneUI()
    {
        _chooseRuneUI.gameObject.SetActive(false);
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