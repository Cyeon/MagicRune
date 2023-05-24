using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RERune : Reward
{
    public override void AddRewardList()
    {
        rewardIcon = Managers.Reward.GetRewardIcon(RewardType.Rune);
        desc = "룬 추가하기";
        isAuto = true;
        Managers.Reward.AddRewardList(this);
    }

    public override void GiveReward()
    {
        Define.DialScene?.ChooseRuneUISetUp();
    }
}
