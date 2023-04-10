using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class REGold : Reward
{
    public int gold;

    public override void AddRewardList()
    {
        icon = Managers.Reward.GetRewardIcon(RewardType.Gold);
        desc = string.Format("{0} °ñµå", gold);
        Managers.Reward.AddRewardList(this);
    }

    public override void GiveReward()
    {
        Managers.Gold.AddGold(gold);
    }
}
