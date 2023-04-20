using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RERune : Reward
{
    private DialScene _dialScene;

    public override void AddRewardList()
    {
        rewardIcon = Managers.Reward.GetRewardIcon(RewardType.Rune);
        desc = "·é Ãß°¡ÇÏ±â";
        Managers.Reward.AddRewardList(this);
    }

    public override void GiveReward()
    {
        Define.DialScene?.ChooseRuneUISetUp();
    }
}
