using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Reward
{
    public Sprite icon;
    public string desc;

    /// <summary>
    /// 보상 지급 함수
    /// </summary>
    public abstract void GiveReward();

    /// <summary>
    /// 보상 목록에 추가
    /// </summary>
    public abstract void AddRewardList();
}