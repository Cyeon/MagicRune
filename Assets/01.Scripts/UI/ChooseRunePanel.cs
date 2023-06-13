using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseRunePanel : ExplainPanel
{
    [SerializeField]
    private RectTransform _deckRectPos;
    [SerializeField]
    private GameObject _effect;
    public void ChooseRune()
    {
        //Managers.Deck.AddRune(_rune);
        Managers.Deck.AddRune(Managers.Rune.GetRune(_rune));
        Define.DialScene?.HideChooseRuneUI();

        UserInfoUI ui = Managers.UI.Get<UserInfoUI>("Upper_Frame");

        Vector3 pos1 = this.transform.position;
        //pos1.z = 100;
        Vector3 pos2 = ui.transform.Find("DeckButton").position;
        //pos1.z = 100;

        BezierMissile b = Managers.Resource.Instantiate("BezierMissile", ui.transform).GetComponent<BezierMissile>();
        b.SetEffect(_effect);
        b.Init(pos1, pos2, 0.8f, 2, 2, () =>
        {
            if (Managers.Reward.IsHaveNextClickReward())
            {
                BattleManager.Instance.NextStage();
            }
        });
    }

    public override void SetUI(BaseRune rune, bool isEnhance = true, bool isReward = true)
    {
        base.SetUI(rune, isEnhance, isReward);
        _rune = rune;

    }
}
