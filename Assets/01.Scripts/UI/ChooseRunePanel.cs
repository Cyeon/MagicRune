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

        // 이펙트 생성
        // 이펙트가 도착하면 아래 코드를 작동
        BezierMissile b = Managers.Resource.Instantiate("BezierMissile").GetComponent<BezierMissile>();
        b.SetEffect(_effect);
        b.Init(Define.MainCam.ScreenToWorldPoint(GetComponent<RectTransform>().anchoredPosition3D + new Vector3(1440f, 2960f) / 2), (Define.MainCam.ScreenToWorldPoint(new Vector3(1440f, 2960f) - _deckRectPos.anchoredPosition3D)), 0.8f, 1, 1, () =>
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
