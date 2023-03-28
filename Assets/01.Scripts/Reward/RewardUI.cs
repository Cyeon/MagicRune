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
    [SerializeField] private GameObject _victoryRewardPanel;

    [SerializeField]
    private GameObject _defeatPanel;

    public void VictoryPanelPopup()
    {
        _backgroundBlur.SetActive(true);
        _victoryPanel.SetActive(true);

        foreach(var reward in RewardManager.GetRewardList())
        {
            RewardPanel panel = ResourceManager.Instance.Instantiate("Reward_Image", _victoryRewardPanel.transform).GetComponent<RewardPanel>();
            panel.transform.localScale = Vector3.one;
            panel.Init(reward);
        }
    }

    public void DefeatPanelPopup()
    {
        _backgroundBlur.SetActive(true);
        _defeatPanel.SetActive(true);
    }
}
