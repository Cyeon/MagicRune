using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class REGold : Reward
{
    public int gold;

    public override void AddRewardList()
    {
        icon = RewardManager.GetRewardIcon(RewardType.Gold);
        desc = string.Format("{0} °ñµå", gold);
        RewardManager.AddRewardList(this);
    }

    public override void GiveReward()
    {
        Managers.Gold.AddGold(gold);
    }
}
