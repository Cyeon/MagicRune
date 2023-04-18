using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class REGold : Reward
{
    private int _gold;

    public override void AddRewardList()
    {
        rewardIcon = Managers.Reward.GetRewardIcon(RewardType.Gold);
        desc = string.Format("{0} 골드", _gold);
        isAuto = true;
        Managers.Reward.AddRewardList(this);
    }

    public override void GiveReward()
    {
        Managers.Gold.AddGold(_gold);
    }

    public void SetGold(int gold)
    {
        _gold = gold;
    }
}
