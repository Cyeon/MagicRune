using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class REGold : Reward
{
    private int _gold;

    public override void AddRewardList()
    {
        icon = Managers.Reward.GetRewardIcon(RewardType.Gold);
        desc = string.Format("{0} 골드", _gold);
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
