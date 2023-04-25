using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum RewardType
{
    Gold,
    Rune
}

public class RewardManager
{
    private static List<Reward> _rewards = new List<Reward>();
    public static Dictionary<RewardType, Sprite> rewardSprites = new Dictionary<RewardType, Sprite>();

    public void ImageLoad()
    {
        rewardSprites.Add(RewardType.Gold, Managers.Resource.Load<Sprite>("Coin_Icon"));
        rewardSprites.Add(RewardType.Rune, Managers.Resource.Load<Sprite>("RuneIcon"));
    }

    public Sprite GetRewardIcon(RewardType type)
    {
        return rewardSprites[type];
    }

    public void ResetRewardList()
    {
        _rewards.Clear();
    }

    public void AddRewardList(Reward reward)
    {
        _rewards.Add(reward);
    }

    public List<Reward> GetRewardList() { return _rewards.Where(x => x.isGive == false && x.isAuto == false).ToList(); }
}
