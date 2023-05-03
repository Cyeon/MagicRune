using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Reward
{
    public Sprite rewardIcon; // 아이콘
    public string desc; // 설명

    // 자동지급 할건지
    public bool isAuto = false;
    public bool isGive = false;

    /// <summary>
    /// 보상 지급 함수
    /// </summary>
    public abstract void GiveReward();

    /// <summary>
    /// 보상 목록에 추가
    /// </summary>
    public abstract void AddRewardList();
}