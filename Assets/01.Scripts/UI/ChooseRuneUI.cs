using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseRuneUI : MonoBehaviour
{
    private List<RewardRunePanel> _rewardPanelList = new List<RewardRunePanel>();

    private void Awake()
    {
        for(int i = 0; i <  transform.childCount; i++)
        {
            _rewardPanelList.Add(transform.GetChild(i).GetComponent<RewardRunePanel>());
        }
    }

    public void SetUp()
    {
        BaseRune[] rune = Managers.Rune.GetRandomRune(3,Managers.Deck.DefaultRune).ToArray();
        for(int i = 0; i < rune.Length; i++)
        {
            _rewardPanelList[i].SetUI(rune[i], false);
        }
    }

    public void Exit()
    {
        gameObject.SetActive(false);
        BattleManager.Instance.NextStage();
    }
}
