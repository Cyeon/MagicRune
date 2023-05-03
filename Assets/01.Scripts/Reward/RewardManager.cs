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
        if(rewardSprites.ContainsKey(RewardType.Gold) == false)
        {
            rewardSprites.Add(RewardType.Gold, Managers.Resource.Load<Sprite>("Coin_Icon"));
        }
        if (rewardSprites.ContainsKey(RewardType.Rune) == false)
        {
            rewardSprites.Add(RewardType.Rune, Managers.Resource.Load<Sprite>("RuneIcon"));
        }
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

    /// <summary>
    /// 지급안된 보상 목록들 불러오는 함수
    /// </summary>
    /// <returns></returns>
    public List<Reward> GetRewardList() { return _rewards; }

    /// <summary>
    /// 자동지급되는 보상들 외에 아직 지급받을 보상이 있는가?
    /// </summary>
    /// <returns></returns>
    public bool IsHaveNextClickReward() { return _rewards.Where(x => x.isGive == false && x.isAuto == false).ToList().Count == 0; }

}
