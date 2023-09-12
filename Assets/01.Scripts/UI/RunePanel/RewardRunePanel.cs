using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardRunePanel : BasicRuneAddon
{
    private ChooseRuneUI _chooseRuneUI;
    private CanvasGroup _canvasGroup;
    private TrailRenderer _trailRenderer;

    [SerializeField]
    private Sprite fireIcon = null;
    [SerializeField]
    private Sprite iceIcon = null;
    [SerializeField]
    private Sprite groundIcon = null;
    [SerializeField]
    private Color _groundColor;
    [SerializeField]
    private Sprite electricIcon = null;
    [SerializeField]
    private Image _attributeIcon = null;
    [SerializeField]
    private TextMeshProUGUI _attributeText = null;

    private void Start()
    {
        _chooseRuneUI = GetComponentInParent<ChooseRuneUI>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void ChooseRune()
    {
        //Managers.Deck.AddRune(Managers.Rune.GetRune(Basic.Rune));
        //Define.DialScene?.HideChooseRuneUI();

        //if (Managers.Reward.IsHaveNextClickReward())
        //{
        //    BattleManager.Instance.NextStage();
        //}

        if (_chooseRuneUI != null)
        {
            _chooseRuneUI.SelectRewardRunePanel(this);
        }
    }

    public override void SetUI(BaseRuneSO baseRuneSO, bool isEnhance = true)
    {
        transform.localScale = Vector3.one;
        _canvasGroup?.DOFade(1, 0);
        if(!_trailRenderer)  _trailRenderer = GetComponent<TrailRenderer>();
        _trailRenderer.enabled = false;

        Basic.SetUI(baseRuneSO, isEnhance);
        switch (baseRuneSO.AttributeType)
        {
            case AttributeType.NonAttribute:
                Debug.Log("무속성은 보상에 없으니까 괜찮아");
                break;
            case AttributeType.Fire:
                _attributeIcon.sprite = fireIcon;
                _attributeText.SetText("불");
                _trailRenderer.startColor = Color.red;
                _trailRenderer.endColor = Color.red;
                break;
            case AttributeType.Ice:
                _attributeIcon.sprite = iceIcon;
                _attributeText.SetText("얼음");
                _trailRenderer.startColor = Color.cyan;
                _trailRenderer.endColor = Color.cyan;
                break;
            case AttributeType.Electric:
                _attributeIcon.sprite = electricIcon;
                _attributeText.SetText("전기");
                _trailRenderer.startColor = Color.yellow;
                _trailRenderer.endColor = Color.yellow;
                break;
            case AttributeType.Ground:
                _attributeIcon.sprite = groundIcon;
                _attributeText.SetText("땅");
                _trailRenderer.startColor = _groundColor;
                _trailRenderer.endColor = _groundColor;
                break;

            case AttributeType.None:
            case AttributeType.MAX_COUNT:
            default:
                break;
        }
        
        if (baseRuneSO.AttributeType == Managers.Rune.GetSelectAttribute())
            _attributeText.color = Color.yellow;
        else
            _attributeText.color = Color.white;

    }

    public override void SetRune(BaseRune rune)
    {
        base.SetRune(rune);
        Basic.ClickAction -= ChooseRune;
        Basic.ClickAction += ChooseRune;
    }

    public void SelectRuneEffect(float speed, Action action)
    {
        Transform deckIcon = Managers.Canvas.GetCanvas("UserInfoPanel").transform.Find("Upper_Frame/DeckButton");
        transform.SetParent(deckIcon.parent.parent);
        transform.GetComponent<RectTransform>().DOAnchorPos3DZ(0, 0);
        _trailRenderer.enabled = true;

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(Vector2.one * 1.2f, 0.24f));
        seq.Append(transform.DOScale(Vector2.one * 0.2f, 0.24f));
        seq.Append(transform.DOMove(deckIcon.position, speed).SetEase(Ease.InCubic));
        seq.Join(_canvasGroup.DOFade(0, speed).SetEase(Ease.InCubic));
        seq.Join(transform.DORotate(transform.position - deckIcon.position, speed));
        seq.Append(deckIcon.DOScale(Vector2.one * 1.2f, 0.2f));
        seq.Append(deckIcon.DOScale(Vector2.one, 0.2f));
        seq.AppendCallback(() =>
        {
            action?.Invoke();
        });
    }
}
