using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseRunePanel : ExplainPanel
{
    private BaseRune _rune;

    public void ChooseRune()
    {
        //Managers.Deck.AddRune(_rune);
        Managers.Deck.AddRune(Managers.Rune.GetRune(_rune));
        Define.DialScene?.HideChooseRuneUI();

        if (Managers.Reward.GetRewardList().Count == 0)
        {
            BattleManager.Instance.NextStage();
        }
    }

    public override void SetUI(BaseRune rune)
    {
        base.SetUI(rune);
        _rune = rune;
    }
}
