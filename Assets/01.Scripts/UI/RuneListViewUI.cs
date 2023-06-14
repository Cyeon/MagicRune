using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuneListViewUI : MonoBehaviour
{
    private GameObject _backgroundPanel = null;
    private GameObject _scrollView = null;

    private Transform _content = null;
    private List<GameObject> _usingPanelList = new List<GameObject>();

    private RuneDial _dial = null;

    private void Start()
    {
        _backgroundPanel = transform.Find("RuneListView_BGPanel").gameObject;
        _scrollView = transform.Find("RuneListView_ScrollView").gameObject;
        _content = _scrollView.transform.GetChild(0).GetChild(0).transform;
        _dial = FindObjectOfType<RuneDial>();

        ActiveUI(false);

        _backgroundPanel.GetComponent<Button>().onClick.AddListener(() =>
        {
            ReturnPanels();
            ActiveUI(false);
        });
    }
    /// <summary>
    /// 쿨타임 중인 룬
    /// </summary>
    public void SetHoldingRunes()
    {
        SettingPanels(Managers.Deck.Deck.FindAll(x => x.IsCoolTime == true), true);
        EventManager.StartListening(Define.ON_START_PLAYER_TURN, UpdateCoolTime);
        ActiveUI(true);
    }
    /// <summary>
    /// 사용할 수 있는 룬
    /// </summary>
    public void SetUseRunes()
    {
        SettingPanels(Managers.Deck.Deck.FindAll(x => x.IsCoolTime == false), false, _dial.ConsumeDeck);
        ActiveUI(true);
    }

    public void SetAllRunes()
    {
        SettingPanels(Managers.Deck.Deck);
        ActiveUI(true);
    }

    private void SettingPanels(List<BaseRune> baseRuneList, bool isCoolTIme = false, List<BaseRune> ignoreRuneList = null)
    {
        ReturnPanels();

        for (int i = 0; i < baseRuneList.Count; i++)
        {
            if (ignoreRuneList.Contains(baseRuneList[i])) { continue; }
            RuneViewPanelUI panel = Managers.Resource.Instantiate("UI/RuneTemplate", _content).GetComponent<RuneViewPanelUI>();
            panel.SetUI(baseRuneList[i], isCoolTIme);
            panel.transform.localScale = Vector3.one;
            _usingPanelList.Add(panel.gameObject);
        }
    }

    public void ReturnPanels()
    {
        foreach (GameObject obj in _usingPanelList)
        {
            Managers.Resource.Destroy(obj);
        }
        _usingPanelList.Clear();
    }

    private void ActiveUI(bool isActive)
    {
        _backgroundPanel.SetActive(isActive);
        _scrollView.SetActive(isActive);
    }

    private void UpdateCoolTime()
    {
        ReturnPanels();
        SettingPanels(Managers.Deck.Deck.FindAll(x => x.IsCoolTime == true), true);
        EventManager.StopListening(Define.ON_START_PLAYER_TURN, UpdateCoolTime);
    }
}