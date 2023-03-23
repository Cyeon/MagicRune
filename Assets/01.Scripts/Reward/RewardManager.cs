using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RewardType
{
    Gold
}

public class RewardManager
{
    private static List<Reward> _rewards = new List<Reward>();
    public static Dictionary<RewardType, Sprite> rewardSprites = new Dictionary<RewardType, Sprite>();

    public static void ImageLoad()
    {
        rewardSprites.Add(RewardType.Gold, Resources.Load<Sprite>("Coin_Icon"));
    }

    public static Sprite GetRewardIcon(RewardType type)
    {
        return rewardSprites[type];
    }

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
