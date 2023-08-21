using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseRuneUI : MonoBehaviour
{
    private List<RewardRunePanel> _rewardPanelList = new List<RewardRunePanel>();
    private RuneRarityType _runeRarityType = RuneRarityType.Base;

    private void Awake()
    {
        for(int i = 0; i <  transform.childCount; i++)
        {
            _rewardPanelList.Add(transform.GetChild(i).GetComponent<RewardRunePanel>());
        }
    }

    public void SetUp()
    {
        switch (Managers.Map.CurrentStage.StageType)
        {
            case StageType.Boss:
                _runeRarityType = RuneRarityType.Boss; break;
            case StageType.Elite:
                _runeRarityType = RuneRarityType.Elite; break;
            default:
                _runeRarityType = RuneRarityType.Base; break;
        }

        BaseRune[] rune = Managers.Rune.GetRandomRune(3, Managers.Deck.DefaultRune, _runeRarityType).ToArray();
        for(int i = 0; i < rune.Length; i++)
        {
            _rewardPanelList[i].SetUI(rune[i].BaseRuneSO, false);
            _rewardPanelList[i].SetRune(rune[i]);
        }
    }

    public void Exit()
    {
        gameObject.SetActive(false);
        BattleManager.Instance.NextStage();
    }
}
