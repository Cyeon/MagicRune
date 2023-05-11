using DG.Tweening;
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
    private Dial _dial;
    public Dial Dial => _dial;

    [SerializeField]
    private GameObject _cardDescPanel;
    private ExplainPanelList _cardDescPanelList;

    [SerializeField]
    private GameObject _statusDescPanel;
    private TextMeshPro _statusDescName;
    private TextMeshPro _statusDescInfo;

    private Slider hSlider = null;
    private Slider sSlider = null;
    private Slider hfSlider = null;
    private TextMeshProUGUI hText = null;

    [SerializeField]
    private RewardUI _rewardUI;
    public RewardUI RewardUI => _rewardUI;
    private ChooseRuneUI _chooseRuneUI;

    private UserInfoUI _userInfoUI;

    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.DialScene;

        #region UI Bind

        Managers.UI.Bind<Image>("TurnBackground", Managers.Canvas.GetCanvas("Popup").gameObject);
        Managers.UI.Bind<TextMeshProUGUI>("TurnText", Managers.Canvas.GetCanvas("Popup").gameObject);
            
        Managers.UI.Bind<Image>("P StatusPanel", Managers.Canvas.GetCanvas("Main").gameObject);
        Managers.UI.Bind<Image>("E StatusPanel", Managers.Canvas.GetCanvas("Main").gameObject);

        Managers.UI.Bind<Slider>("P HealthBar", Managers.Canvas.GetCanvas("Main").gameObject);
        Managers.UI .Bind<Slider>("P ShieldBar", Managers.Canvas.GetCanvas("Main").gameObject);
        Managers.UI.Bind<TextMeshProUGUI>("P HealthText", Managers.Canvas.GetCanvas("Main").gameObject);
        Managers.UI.Bind<Slider>("P HealthFeedbackBar", Managers.Canvas.GetCanvas("Main").gameObject);
        Managers.UI.Bind<TextMeshProUGUI>("P Shield Value", Managers.Canvas.GetCanvas("Main").gameObject);

        Managers.UI.Bind<Button>("Restart Btn", Managers.Canvas.GetCanvas("Popup").gameObject);
        Managers.UI.Bind<Button>("Quit Btn", Managers.Canvas.GetCanvas("Popup").gameObject);

        Managers.UI.Bind<TextMeshPro>("Status_Name_Text", Managers.Canvas.GetCanvas("Popup").gameObject);
        Managers.UI.Bind<TextMeshPro>("Status_Infomation_Text", Managers.Canvas.GetCanvas("Popup").gameObject);

        Managers.UI.Bind<ChooseRuneUI>("ChooseRuneUI", Managers.Canvas.GetCanvas("Popup").gameObject);

        Managers.UI.Bind<UserInfoUI>("Upper_Frame", Managers.Canvas.GetCanvas("UserInfoPanelCanvas").gameObject);

        #endregion

        _statusDescName = Managers.UI.Get<TextMeshPro>("Status_Name_Text");
        _statusDescInfo = Managers.UI.Get<TextMeshPro>("Status_Infomation_Text");

        _chooseRuneUI = Managers.UI.Get<ChooseRuneUI>("ChooseRuneUI").GetComponent<ChooseRuneUI>();

        //UIManager.Instance.Get<Button>("Restart Btn").onClick.RemoveAllListeners();
        //UIManager.Instance.Get<Button>("Restart Btn").onClick.AddListener(() =>
        //{
        //    SceneManagerEX.Instance.LoadScene(Define.Scene.MapScene);
        //    //MapManager.Instance.ResetChapter();
        //});
        Managers.UI.Get<Button>("Quit Btn").onClick.RemoveAllListeners();
        Managers.UI.Get<Button>("Quit Btn").onClick.AddListener(() => Managers.GameQuit());

        _cardDescPanelList = _cardDescPanel.GetComponent<ExplainPanelList>();

        Managers.Sound.PlaySound("BGM/DialSceneBGM", SoundType.Bgm, true, 1.0f);

        Debug.Log($"Resolution : {Screen.width}, {Screen.height}");
        _userInfoUI = Managers.UI.Get<UserInfoUI>("Upper_Frame");

        _userInfoUI.UpdateHealthText();
        _userInfoUI.UpdateGoldText();

        Managers.Gold.UpdateGoldAction -= _userInfoUI.UpdateGoldText;
        Managers.Gold.UpdateGoldAction += _userInfoUI.UpdateGoldText;

        Managers.GetPlayer().userInfoUI = _userInfoUI;
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
        for(int i = trm.childCount - 1; i >= 0; --i)
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
        if(rune == null)
        {
            CardDescDown();
        }
        else
        {
            _cardDescPanel.SetActive(true);
            if (_cardDescPanelList == null)
            {
                _cardDescPanelList = _cardDescPanel.GetComponent<ExplainPanelList>();
            }

            _cardDescPanelList.OpenPanel(0, rune);
            _cardDescPanelList.ClosePanel(1);
            _cardDescPanelList.ClosePanel(2);
        }
    }

    public void AllCardDescPopup()
    {
        _cardDescPanel.SetActive(true);
        if (_cardDescPanelList == null)
        {
            _cardDescPanelList = _cardDescPanel.GetComponent<ExplainPanelList>();
        }

        for (int i = 0; i < _dial.DialElementList.Count; i++)
        {
            if (_dial.DialElementList[i].SelectCard != null)
            {
                _cardDescPanelList.OpenPanel(_dial.DialElementList.Count - 1 - i, _dial.DialElementList[i].SelectCard.Rune);
            }
            else
            {
                _cardDescPanelList.ClosePanel(i);
            }
        }
    }

    public void CardDescDown()
    {
        _cardDescPanel.SetActive(false);
    }

    public void StatusDescPopup(Status status, Vector3 pos, bool isDown = true)
    {
        if (isDown == false)
        {
            _statusDescPanel.SetActive(false);
            return;
        }

        _statusDescPanel.SetActive(true);
        _statusDescPanel.transform.position = pos + new Vector3(0, 2, 0);
        _statusDescPanel.transform.DOMoveZ(-1, 0);

        _statusDescName.text = status.debugName;
        _statusDescInfo.text = status.information;
    }

    #endregion

    #region Health Bar

    public void SliderInit(bool isPlayer)
    {
        if (isPlayer)
        {
            hSlider = Managers.UI.Get<Slider>("P HealthBar");
            sSlider = Managers.UI.Get<Slider>("P ShieldBar");
            hText = Managers.UI.Get<TextMeshProUGUI>("P HealthText");
            hfSlider = Managers.UI.Get<Slider>("P HealthFeedbackBar");
        }
        else
        {
            hSlider = Managers.UI.Get<Slider>("E HealthBar");
            sSlider = Managers.UI.Get<Slider>("E ShieldBar");
            hText = Managers.UI.Get<TextMeshProUGUI>("E HealthText");
            hfSlider = Managers.UI.Get<Slider>("E HealthFeedbackBar");
        }
    }

    public void HealthbarInit(bool isPlayer, float health, float maxHealth = 0)
    {
        SliderInit(isPlayer);

        if (maxHealth == 0)
            maxHealth = health;

        hSlider.maxValue = maxHealth;
        hSlider.value = health;
        hText.text = string.Format("{0} / {1}", health, maxHealth);

        sSlider.maxValue = maxHealth;
        sSlider.value = 0;

        hfSlider.maxValue = maxHealth;
        hfSlider.value = 0;
    }

    public void UpdateHealthbar(bool isPlayer)
    {
        SliderInit(isPlayer);
        Unit unit = isPlayer ? BattleManager.Instance.Player : BattleManager.Instance.Enemy;
        if (unit == null) return;

        if (unit.Shield > 0)
        {
            if (unit.HP + unit.Shield > unit.MaxHP)
                hfSlider.value = hSlider.maxValue = sSlider.maxValue = hfSlider.maxValue = unit.HP + unit.Shield;
            else
                hSlider.maxValue = sSlider.maxValue = hfSlider.maxValue = unit.MaxHP;

            hfSlider.value = hSlider.value;
            hSlider.value = unit.HP;
            sSlider.value = unit.HP + unit.Shield;
            hText.text = string.Format("{0} / {1}", hSlider.value, unit.MaxHP);
        }
        else
        {
            hSlider.maxValue = sSlider.maxValue = hfSlider.maxValue = unit.MaxHP;
            sSlider.value = 0;
            hSlider.value = unit.HP;
            hText.text = string.Format("{0} / {1}", hSlider.value, unit.MaxHP);
        }

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.5f);
        seq.Append(hfSlider.DOValue(unit.HP, 0.2f));

        Sequence vibrateSeq = DOTween.Sequence();
        vibrateSeq.Append(hSlider.transform.parent.DOShakeScale(0.1f));
        vibrateSeq.Append(hSlider.transform.parent.DOScale(1f, 0));
    }
    #endregion


    public void UpdateShieldText(float shield)
    {
        TextMeshProUGUI playerShieldText = Managers.UI.Get<TextMeshProUGUI>("P Shield Value");

        playerShieldText.SetText(shield.ToString());

        Sequence seq = DOTween.Sequence();
        seq.Append(playerShieldText.transform.parent.DOScale(1.4f, 0.1f));
        seq.Append(playerShieldText.transform.parent.DOScale(1f, 0.1f));

        UpdateHealthbar(true);
    }

    public override void Clear()
    {
        // �� Ŭ����
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

}
