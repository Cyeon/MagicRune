using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseRuneUI : MonoBehaviour
{
    private List<RewardRunePanel> _rewardPanelList = new List<RewardRunePanel>();
    private RuneRarityType _runeRarityType = RuneRarityType.Base;

    private Button _giveBtn;
    private RewardRunePanel _selectRewardRunePanel;

    private void Awake()
    {
        for(int i = 0; i <  transform.childCount; i++)
        {
            if(transform.GetChild(i).TryGetComponent<RewardRunePanel>(out RewardRunePanel panel))
            {
                _rewardPanelList.Add(transform.GetChild(i).GetComponent<RewardRunePanel>());
            }
        }
        _giveBtn = transform.Find("Give_Button").GetComponent<Button>();
        _giveBtn.onClick.RemoveAllListeners();
        _giveBtn.onClick.AddListener(() =>
        {
            Managers.Deck.AddRune(Managers.Rune.GetRune(_selectRewardRunePanel.Basic.Rune));
            Define.DialScene?.HideChooseRuneUI();

            if (Managers.Reward.IsHaveNextClickReward())
            {
                BattleManager.Instance.NextStage();
            }
        });
    }

    public void SelectRewardRunePanel(RewardRunePanel rewardPanel)
    {
        // rewardPanel의 테두리 끄기( 널이 아니면)
        _selectRewardRunePanel = rewardPanel;
        // rewardPanel의 테두리 키기(널이 아니면)
    }

    public void SetUp()
    {
        switch (Managers.Map.currentStage.StageType)
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
