using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _backgroundBlur;

    [SerializeField]
    private GameObject _victoryPanel;
    [SerializeField] private GameObject _rewardPanel;

    private void Start()
    {
        PoolManager.CreatePool<RewardPanel>("Reward_Image", _rewardPanel, 4);
    }

    public void VictoryPanelPopup()
    {
        _backgroundBlur.SetActive(true);
        _victoryPanel.SetActive(true);

        foreach(var reward in RewardManager.GetRewardList())
        {
            RewardPanel panel = PoolManager.GetItem<RewardPanel>("Reward_Image");
            panel.Init(reward);
        }
    }
}
