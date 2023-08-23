using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardRunePanel : BasicRuneAddon
{
    private ChooseRuneUI _chooseRuneUI;

    private void Start()
    {
        _chooseRuneUI = GetComponentInParent<ChooseRuneUI>();
    }
    [SerializeField]
    private Image _attributeIcon = null;
    [SerializeField]
    private TextMeshProUGUI _attributeText = null;

    public void ChooseRune()
    {
        //Managers.Deck.AddRune(Managers.Rune.GetRune(Basic.Rune));
        //Define.DialScene?.HideChooseRuneUI();

        //if (Managers.Reward.IsHaveNextClickReward())
        //{
        //    BattleManager.Instance.NextStage();
        //}

        if(_chooseRuneUI != null)
        {
            _chooseRuneUI.SelectRewardRunePanel(this);
        }
    }

    public override void SetUI(BaseRuneSO baseRuneSO, bool isEnhance = true)
    {
        Basic.SetUI(baseRuneSO, isEnhance);
        switch (baseRuneSO.AttributeType)
        {
            case AttributeType.NonAttribute:
                Debug.Log("무속성은 보상에 없으니까 괜찮아");
                break;
            case AttributeType.Fire:
                _attributeIcon.sprite = Managers.Resource.Load<Sprite>("Sprite/Attribute/Fire");
                _attributeText.SetText("불");
                break;
            case AttributeType.Ice:
                _attributeIcon.sprite = Managers.Resource.Load<Sprite>("Sprite/Attribute/Ice");
                _attributeText.SetText("얼음");
                break;
            case AttributeType.Electric:
                _attributeIcon.sprite = Managers.Resource.Load<Sprite>("Sprite/Attribute/Electric");
                _attributeText.SetText("전기");
                break;
            case AttributeType.Ground:
                _attributeIcon.sprite = Managers.Resource.Load<Sprite>("Sprite/Attribute/Ground");
                _attributeText.SetText("땅");
                break;

            case AttributeType.None:
            case AttributeType.MAX_COUNT:
            default:
                break;
        }
    }

    public override void SetRune(BaseRune rune)
    {
        base.SetRune(rune);
        Basic.ClickAction -= ChooseRune;
        Basic.ClickAction += ChooseRune;
    }
}
