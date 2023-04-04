using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialScene : BaseScene
{
    [SerializeField]
    private Dial _dial;
    public Dial Dial => _dial;

    [SerializeField]
    private SpriteRenderer _enemyIcon;
    public SpriteRenderer EnemyIcon => _enemyIcon;

    private Image _enemyPatternIcon;
    private TextMeshProUGUI _enemyPatternValueText;

    [SerializeField]
    private GameObject _cardDescPanel;
    private TextMeshProUGUI _cardDescName;
    private Image _cardDescSkillIcon;
    private TextMeshProUGUI _cardDescInfo;
    private TextMeshProUGUI _cardDescCoolTime;

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

    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.DialScene;

        #region UI Bind

        UIManager.Instance.Bind<Image>("TurnBackground", CanvasManager.Instance.GetCanvas("Popup").gameObject);
        UIManager.Instance.Bind<TextMeshProUGUI>("TurnText", CanvasManager.Instance.GetCanvas("Popup").gameObject);

        UIManager.Instance.Bind<Image>("P StatusPanel", CanvasManager.Instance.GetCanvas("Main").gameObject);
        UIManager.Instance.Bind<Image>("E StatusPanel", CanvasManager.Instance.GetCanvas("Main").gameObject);

        UIManager.Instance.Bind<Image>("NextPattern Image", CanvasManager.Instance.GetCanvas("Main").gameObject);
        UIManager.Instance.Bind<TextMeshProUGUI>("NextPattern ValueText", CanvasManager.Instance.GetCanvas("Main").gameObject);

        UIManager.Instance.Bind<TextMeshProUGUI>("Skill_Name_Text", CanvasManager.Instance.GetCanvas("Popup").gameObject);
        UIManager.Instance.Bind<Image>("Explain_Skill_Icon", CanvasManager.Instance.GetCanvas("Popup").gameObject);
        UIManager.Instance.Bind<TextMeshProUGUI>("Explain_Text", CanvasManager.Instance.GetCanvas("Popup").gameObject);
        UIManager.Instance.Bind<TextMeshProUGUI>("CoolTime_Text", CanvasManager.Instance.GetCanvas("Popup").gameObject);

        UIManager.Instance.Bind<Slider>("E HealthBar", CanvasManager.Instance.GetCanvas("Main").gameObject);
        UIManager.Instance.Bind<Slider>("E ShieldBar", CanvasManager.Instance.GetCanvas("Main").gameObject);
        UIManager.Instance.Bind<TextMeshProUGUI>("E HealthText", CanvasManager.Instance.GetCanvas("Main").gameObject);
        UIManager.Instance.Bind<Slider>("E HealthFeedbackBar", CanvasManager.Instance.GetCanvas("Main").gameObject);
        UIManager.Instance.Bind<TextMeshProUGUI>("E Shield Value", CanvasManager.Instance.GetCanvas("Main").gameObject);

        UIManager.Instance.Bind<Slider>("P HealthBar", CanvasManager.Instance.GetCanvas("Main").gameObject);
        UIManager.Instance.Bind<Slider>("P ShieldBar", CanvasManager.Instance.GetCanvas("Main").gameObject);
        UIManager.Instance.Bind<TextMeshProUGUI>("P HealthText", CanvasManager.Instance.GetCanvas("Main").gameObject);
        UIManager.Instance.Bind<Slider>("P HealthFeedbackBar", CanvasManager.Instance.GetCanvas("Main").gameObject);
        UIManager.Instance.Bind<TextMeshProUGUI>("P Shield Value", CanvasManager.Instance.GetCanvas("Main").gameObject);

        UIManager.Instance.Bind<Button>("Restart Btn", CanvasManager.Instance.GetCanvas("Popup").gameObject);
        UIManager.Instance.Bind<Button>("Quit Btn", CanvasManager.Instance.GetCanvas("Popup").gameObject);

        UIManager.Instance.Bind<TextMeshPro>("Status_Name_Text", CanvasManager.Instance.GetCanvas("Popup").gameObject);
        UIManager.Instance.Bind<TextMeshPro>("Status_Infomation_Text", CanvasManager.Instance.GetCanvas("Popup").gameObject);

        #endregion

        _enemyPatternIcon = UIManager.Instance.Get<Image>("NextPattern Image");
        _enemyPatternValueText = UIManager.Instance.Get<TextMeshProUGUI>("NextPattern ValueText");

        _cardDescName = UIManager.Instance.Get<TextMeshProUGUI>("Skill_Name_Text");
        _cardDescSkillIcon = UIManager.Instance.Get<Image>("Explain_Skill_Icon");
        _cardDescInfo = UIManager.Instance.Get<TextMeshProUGUI>("Explain_Text");
        _cardDescCoolTime = UIManager.Instance.Get<TextMeshProUGUI>("CoolTime_Text");

        _statusDescName = UIManager.Instance.Get<TextMeshPro>("Status_Name_Text");
        _statusDescInfo = UIManager.Instance.Get<TextMeshPro>("Status_Infomation_Text");

        //UIManager.Instance.Get<Button>("Restart Btn").onClick.RemoveAllListeners();
        //UIManager.Instance.Get<Button>("Restart Btn").onClick.AddListener(() =>
        //{
        //    SceneManagerEX.Instance.LoadScene(Define.Scene.MapScene);
        //    //MapManager.Instance.ResetChapter();
        //});
        UIManager.Instance.Get<Button>("Quit Btn").onClick.RemoveAllListeners();
        UIManager.Instance.Get<Button>("Quit Btn").onClick.AddListener(() => GameManager.Instance.GameQuit());
    }

    public void Turn(string text)
    {
        Image turnBG = UIManager.Instance.Get<Image>("TurnBackground");
        TextMeshProUGUI turnText = UIManager.Instance.Get<TextMeshProUGUI>("TurnText");

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
        StatusPanel statusPanel = ResourceManager.Instance.Instantiate("Status").GetComponent<StatusPanel>();
        statusPanel.status = status;

        statusPanel.image.sprite = status.icon;
        statusPanel.image.color = status.color;
        statusPanel.duration.text = status.typeValue.ToString();
        statusPanel.statusName = status.statusName;
        statusPanel.transform.SetParent(parent);
        statusPanel.transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);

        return statusPanel;
    }

    public void AddStatus(Unit unit, Status status)
    {
        Transform trm = unit == BattleManager.Instance.player ?
            UIManager.Instance.Get<Image>("P StatusPanel").transform : UIManager.Instance.Get<Image>("E StatusPanel").transform;
        GetStatusPanel(status, trm);
    }

    public GameObject GetStatusPanelStatusObj(Unit unit, StatusName name)
    {
        Transform trm = unit == BattleManager.Instance.player ?
            UIManager.Instance.Get<Image>("P StatusPanel").transform : UIManager.Instance.Get<Image>("E StatusPanel").transform;

        GameObject obj = null;
        for (int i = 0; i < trm.childCount; i++)
        {
            if (trm.GetChild(i).GetComponent<StatusPanel>().statusName == name)
            {
                obj = trm.GetChild(i).gameObject;
            }
        }

        return obj;
    }

    public void ReloadStatusPanel(Unit unit, StatusName name, int duration)
    {
        GameObject obj = GetStatusPanelStatusObj(unit, name);

        if (obj == null)
            return;

        if (duration <= 0)
        {
            RemoveStatusPanel(unit, name);
            return;
        }

        obj.GetComponent<StatusPanel>().duration.text = duration.ToString();
    }

    public void RemoveStatusPanel(Unit unit, StatusName name)
    {
        GameObject obj = GetStatusPanelStatusObj(unit, name);

        if (obj == null)
        {
            return;
        }

        ResourceManager.Instance.Destroy(obj);
    }

    #endregion

    #region Popup
    public void DamageUIPopup(float amount, Vector3 pos, Status status = null)
    {
        DamagePopup popup = ResourceManager.Instance.Instantiate("DamagePopup", CanvasManager.Instance.GetCanvas("Popup").transform).GetComponent<DamagePopup>();
        popup.Setup(amount, pos, status);
    }

    public void StatusPopup(Status status)
    {
        GameObject obj = ResourceManager.Instance.Instantiate("StatusPopup", _enemyIcon.transform);
        Image img = obj.GetComponent<Image>();
        img.sprite = status.icon;
        obj.transform.localScale = Vector3.one * 8f;

        Sequence seq = DOTween.Sequence();
        seq.Append(obj.transform.DOScale(9f, 0.7f).SetEase(Ease.InQuart));
        seq.Join(img.DOFade(0, 0.7f).SetEase(Ease.InQuart));
        seq.AppendCallback(() =>
        {
            ResourceManager.Instance.Destroy(obj);
        });

        GameObject textPopup = ResourceManager.Instance.Instantiate("StatusTextPopup", _enemyIcon.transform);
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
            ResourceManager.Instance.Destroy(textPopup);
        });
    }

    public void InfoMessagePopup(string message, Vector3 pos)
    {
        InfoMessage popup = ResourceManager.Instance.Instantiate("InfoMessage", CanvasManager.Instance.GetCanvas("Popup").transform).GetComponent<InfoMessage>();
        pos.z = 0;
        pos.y += 1;
        popup.Setup(message, pos);
    }
    #endregion

    #region Pattern
    public void ReloadPattern(Sprite sprite, string value = "")
    {
        _enemyPatternIcon.sprite = sprite;
        _enemyPatternValueText.text = value;
    }

    public IEnumerator PatternIconAnimationCoroutine()
    {
        for (int i = 0; i < 6; i++)
        {
            GameObject obj = Instantiate(_enemyPatternIcon.gameObject, _enemyPatternIcon.transform.parent);
            obj.transform.position = _enemyPatternIcon.transform.position;
            PatternIconAnimation(obj);
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void PatternIconAnimation(GameObject obj)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(obj.transform.DOScale(2f, 0.5f));
        seq.Join(obj.GetComponent<Image>().DOFade(0, 0.5f));
        seq.AppendCallback(() => Destroy(obj));
    }
    #endregion

    #region Description

    public void CardDescPopup(Rune rune)
    {
        if (rune == null)
        {
            _cardDescPanel.SetActive(false);
        }
        else
        {
            RuneSO magic = rune.GetRune();

            _cardDescName.SetText(magic.Name);
            _cardDescSkillIcon.sprite = magic.RuneImage;
            _cardDescInfo.SetText(magic.MainRune.CardDescription);
            _cardDescCoolTime.SetText(magic.CoolTime.ToString());
            _cardDescPanel.SetActive(true);
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

    public void StatusDescDown()
    {
        _statusDescPanel.SetActive(false);
    }

    #endregion

    #region Health Bar

    public void SliderInit(bool isPlayer)
    {
        if (isPlayer)
        {
            hSlider = UIManager.Instance.Get<Slider>("P HealthBar");
            sSlider = UIManager.Instance.Get<Slider>("P ShieldBar");
            hText = UIManager.Instance.Get<TextMeshProUGUI>("P HealthText");
            hfSlider = UIManager.Instance.Get<Slider>("P HealthFeedbackBar");
        }
        else
        {
            hSlider = UIManager.Instance.Get<Slider>("E HealthBar");
            sSlider = UIManager.Instance.Get<Slider>("E ShieldBar");
            hText = UIManager.Instance.Get<TextMeshProUGUI>("E HealthText");
            hfSlider = UIManager.Instance.Get<Slider>("E HealthFeedbackBar");
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
        Unit unit = isPlayer ? BattleManager.Instance.player : BattleManager.Instance.enemy;

        if (unit.Shield > 0)
        {
            if (unit.HP + unit.Shield > unit.MaxHealth)
                hfSlider.value = hSlider.maxValue = sSlider.maxValue = hfSlider.maxValue = unit.HP + unit.Shield;
            else
                hSlider.maxValue = sSlider.maxValue = hfSlider.maxValue = unit.MaxHealth;

            hfSlider.value = hSlider.value;
            hSlider.value = unit.HP;
            sSlider.value = unit.HP + unit.Shield;
            hText.text = string.Format("{0} / {1}", hSlider.value, unit.MaxHealth);
        }
        else
        {
            hSlider.maxValue = sSlider.maxValue = hfSlider.maxValue = unit.MaxHealth;
            sSlider.value = 0;
            hSlider.value = unit.HP;
            hText.text = string.Format("{0} / {1}", hSlider.value, unit.MaxHealth);
        }

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.5f);
        seq.Append(hfSlider.DOValue(unit.HP, 0.2f));

        Sequence vibrateSeq = DOTween.Sequence();
        vibrateSeq.Append(hSlider.transform.parent.DOShakeScale(0.1f));
        vibrateSeq.Append(hSlider.transform.parent.DOScale(1f, 0));
    }
    #endregion


    public void UpdateShieldText(bool isPlayer, float shield)
    {
        if (isPlayer)
        {
            TextMeshProUGUI playerShieldText = UIManager.Instance.Get<TextMeshProUGUI>("P Shield Value");

            playerShieldText.SetText(shield.ToString());

            Sequence seq = DOTween.Sequence();
            seq.Append(playerShieldText.transform.parent.DOScale(1.4f, 0.1f));
            seq.Append(playerShieldText.transform.parent.DOScale(1f, 0.1f));
        }
        else
        {
            TextMeshProUGUI enemyShieldText = UIManager.Instance.Get<TextMeshProUGUI>("E Shield Value");

            enemyShieldText.SetText(shield.ToString());

            Sequence seq = DOTween.Sequence();
            seq.Append(enemyShieldText.transform.parent.DOScale(1.2f, 0.1f));
            seq.Append(enemyShieldText.transform.parent.DOScale(1f, 0.1f));
        }

        UpdateHealthbar(isPlayer);
    }

    public override void Clear()
    {
        // ·é Å¬¸®¾î
    }
}
