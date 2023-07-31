using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class RewardRunePanel : BasicRuneAddon
{
    public void ChooseRune()
    {
        Managers.Deck.AddRune(Managers.Rune.GetRune(Basic.Rune));
        Define.DialScene?.HideChooseRuneUI();

        if (Managers.Reward.IsHaveNextClickReward())
        {
            BattleManager.Instance.NextStage();
        }
    }

    public override void SetUI(BaseRuneSO baseRuneSO, bool isEnhance = true)
    {
        Basic.SetUI(baseRuneSO, isEnhance);
    }

    public override void SetRune(BaseRune rune)
    {
        base.SetRune(rune);
        Basic.ClickAction -= ChooseRune;
        Basic.ClickAction += ChooseRune;
    }
}
