using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Reward
{
    public Sprite icon;
    public string desc;

    /// <summary>
    /// ���� ���� �Լ�
    /// </summary>
    public abstract void GiveReward();

    /// <summary>
    /// ���� ��Ͽ� �߰�
    /// </summary>
    public abstract void AddRewardList();
}