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

            _selectRewardRunePanel.GetComponent<KeywardRunePanel>().ClearKeyward();
        }
        _selectRewardRunePanel = _selectRewardRunePanel == rewardPanel ? null : rewardPanel;
        if(_selectRewardRunePanel != null)
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(_selectRewardRunePanel.GetComponent<RectTransform>().DOAnchorPosY(100, 0.1f).SetRelative());
            seq.Join(_selectRewardRunePanel.GetComponent<RectTransform>().DOScale(Vector3.one * 1.1f, 0.1f));
            _selectRewardRunePanel.GetComponent<RectTransform>().SetAsLastSibling();
            _selectRewardRunePanel.GetComponent<KeywardRunePanel>().SetKeyward(_selectRewardRunePanel.Basic.Rune.BaseRuneSO);
        }
    }

    public void SetUp()
    {
        BaseRune[] rune = Managers.Rune.GetRandomRune(3, Managers.Deck.DefaultRune).ToArray();

        _rewardPanelList.ForEach(x =>
        {
            x.transform.DORotate(new Vector3(-15, -75, 0), 0);
            x.Basic.CanvasGroup.alpha = 0;
         });

        for(int i = 0; i < rune.Length; i++)
        {
            _rewardPanelList[i].SetUI(rune[i].BaseRuneSO, false);
            _rewardPanelList[i].SetRune(rune[i]);
        }

        Sequence seq = DOTween.Sequence();
        seq.Append(_rewardPanelList[0].transform.DORotate(new Vector3(0, 0, 0), 0.42f));
        seq.Join(_rewardPanelList[0].Basic.CanvasGroup.DOFade(1, 0.42f));

        for(int i = 1; i < rune.Length; i++)
        {
            seq.Insert(0.175f * i, _rewardPanelList[i].transform.DORotate(new Vector3(0, 0, 0), 0.35f));
            seq.Insert(0.175f * i, _rewardPanelList[i].Basic.CanvasGroup.DOFade(1, 0.35f));
        }
    }

    public void Exit()
    {
        gameObject.SetActive(false);
        BattleManager.Instance.NextStage();
    }
}
