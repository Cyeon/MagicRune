using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseRunePanel : ExplainPanel
{
    private BaseRune _rune;

    public void ChooseRune()
    {
        Managers.Deck.AddRune(_rune);
        Define.DialScene?.HideChooseRuneUI();
    }

    public override void SetUI(BaseRune rune)
    {
        Debug.Log(rune.BaseRuneSO.RuneName);
        base.SetUI(rune);
        _rune = rune;
    }
}
