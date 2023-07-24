using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Rune 도감에 사용되는 카드 프리팹에 붙는 스크립트 
/// </summary>
public class RuneBookPanel : BasicRuneAddon
{
    private void SetUI(DiscoveryType type)
    {
        switch (type)
        {
            case DiscoveryType.Unknwon:
                Basic.NameText.SetText("???");
                Basic.DescText.SetText("알 수 없음");
                Basic.CoolTImeText.SetText("?");
                Basic.RuneIcon.sprite = Managers.Resource.Load<Sprite>("Sprite/QuestionIcon");
                break;
            case DiscoveryType.Find:
                Basic.NameText.SetText("???");
                Basic.DescText.SetText("알 수 없음");
                Basic.CoolTImeText.SetText("?");
                break;
            case DiscoveryType.Known:
            default:
                break;
        }
    }

    public override void SetUI(BaseRuneSO runeSO, bool isEnhance = true)
    {
        base.SetUI(runeSO, isEnhance);
        SetUI(runeSO.DiscoveryType);
    }
}