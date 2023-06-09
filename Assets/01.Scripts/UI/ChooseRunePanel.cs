using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseRunePanel : ExplainPanel
{
    public void ChooseRune()
    {
        //Managers.Deck.AddRune(_rune);
        Managers.Deck.AddRune(Managers.Rune.GetRune(_rune));
        Define.DialScene?.HideChooseRuneUI();

        if (Managers.Reward.IsHaveNextClickReward())
        {
            BattleManager.Instance.NextStage();
        }
    }

    public override void SetUI(BaseRune rune, bool isEnhance = true, bool isReward = true)
    {
        base.SetUI(rune, isEnhance, isReward);
        _rune = rune;

    }
}
