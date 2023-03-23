using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager
{
    private static List<Reward> _rewards = new List<Reward>();

    public static void ResetRewardList()
    {
        _rewards.Clear();
    }

    public static void AddRewardList(Reward reward)
    {
        _rewards.Add(reward);
    }

    public static List<Reward> GetRewardList() { return _rewards; }
}
