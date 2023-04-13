using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RERune : Reward
{
    private DialScene _dialScene;

    public override void AddRewardList()
    {
        icon = Managers.Reward.GetRewardIcon(RewardType.Rune);
        desc = "·é Ãß°¡ÇÏ±â";
        Managers.Reward.AddRewardList(this);
    }

    public override void GiveReward()
    {
        _dialScene = Managers.Scene.CurrentScene as DialScene;
        _dialScene?.ChooseRuneUISetUp();

    }
}
