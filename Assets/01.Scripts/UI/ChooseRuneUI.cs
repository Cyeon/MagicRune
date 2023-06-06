using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseRuneUI : MonoBehaviour
{
    private List<ChooseRunePanel> _crPanelList = new List<ChooseRunePanel>();

    private void Awake()
    {
        Transform trm = transform.Find("ChooseRuneList");
        for(int i = 0; i <  trm.childCount; i++)
        {
            _crPanelList.Add(trm.GetChild(i).GetComponent<ChooseRunePanel>());
        }
    }

    public void SetUp()
    {
        foreach(var panel  in _crPanelList)
        {
            BaseRune rune = Managers.Rune.GetRandomRune();
            panel.SetUI(rune, isEnhance:false);
        }
    }

    public void Exit()
    {
        gameObject.SetActive(false);
        BattleManager.Instance.NextStage();

        //if(Managers.Reward.IsHaveNextClickReward())
        //{
        //   BattleManager.Instance.NextStage();
        //}
    }
}
