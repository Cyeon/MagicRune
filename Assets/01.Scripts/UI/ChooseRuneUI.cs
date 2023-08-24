using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ChooseRuneUI : MonoBehaviour
{
    private List<RewardRunePanel> _rewardPanelList = new List<RewardRunePanel>();

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
            if (_selectRewardRunePanel != null)
            {
                Managers.Deck.AddRune(Managers.Rune.GetRune(_selectRewardRunePanel.Basic.Rune));
                Define.DialScene?.HideChooseRuneUI();

                if (Managers.Reward.IsHaveNextClickReward())
                {
                    BattleManager.Instance.NextStage();
                }
            }
        });
    }

    public void SelectRewardRunePanel(RewardRunePanel rewardPanel)
    {
        _selectRewardRunePanel.DOComplete();

        if (_selectRewardRunePanel != null)
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(_selectRewardRunePanel.GetComponent<RectTransform>().DOAnchorPosY(-100, 0.1f).SetRelative());
            seq.Join(_selectRewardRunePanel.GetComponent<RectTransform>().DOScale(Vector3.one, 0.1f));
        }
        _selectRewardRunePanel = _selectRewardRunePanel == rewardPanel ? null : rewardPanel;
        if(_selectRewardRunePanel != null)
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(_selectRewardRunePanel.GetComponent<RectTransform>().DOAnchorPosY(100, 0.1f).SetRelative());
            seq.Join(_selectRewardRunePanel.GetComponent<RectTransform>().DOScale(Vector3.one * 1.1f, 0.1f));
        }
    }

    public void SetUp()
    {
        BaseRune[] rune = Managers.Rune.GetRandomRune(3, Managers.Deck.DefaultRune).ToArray();
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
