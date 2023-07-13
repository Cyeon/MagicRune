using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class RewardRunePanel : BasicRunePanel
{
    public void ChooseRune()
    {
        Managers.Deck.AddRune(Managers.Rune.GetRune(Rune));
        Define.DialScene?.HideChooseRuneUI();

        if (Managers.Reward.IsHaveNextClickReward())
        {
            BattleManager.Instance.NextStage();
        }
    }

    public override void SetUI(BaseRune rune, bool isEnhance = true)
    {
        base.SetUI(rune, isEnhance);
        ClickAction += ChooseRune;
    }
}
