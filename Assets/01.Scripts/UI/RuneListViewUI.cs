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

    private void Start()
    {
        _backgroundPanel = transform.Find("RuneListView_BGPanel").gameObject;
        _scrollView = transform.Find("RuneListView_ScrollView").gameObject;
        _content = _scrollView.transform.Find("Viewport").GetChild(0).transform;

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
        SettingPanels(Managers.Deck.Deck.FindAll(x => x.IsCoolTime == true), null, true);
        EventManager.StartListening(Define.ON_START_PLAYER_TURN, UpdateCoolTime);
        ActiveUI(true);
    }
    /// <summary>
    /// 사용할 수 있는 룬
    /// </summary>
    public void SetUseRunes()
    {
        SettingPanels(Managers.Deck.Deck.FindAll(x => x.IsCoolTime == false), Define.DialScene?.Dial.ConsumeDeck);
        ActiveUI(true);
    }

    public void SetAllRunes()
    {
        SettingPanels(Managers.Deck.Deck);
        ActiveUI(true);
    }

    private void SettingPanels(List<BaseRune> baseRuneList, List<BaseRune> ignoreRuneList = null, bool isCoolTime = false)
    {
        ReturnPanels();

        for (int i = 0; i < baseRuneList.Count; i++)
        {
            if (ignoreRuneList != null)
                if (ignoreRuneList.Contains(baseRuneList[i])) { continue; }
            CoolTimeRunePanel panel = Managers.Resource.Instantiate("UI/RunePanel/CoolTime", _content).GetComponent<CoolTimeRunePanel>();
            panel.SetUI(baseRuneList[i].BaseRuneSO, baseRuneList[i].IsEnhanced);
            panel.transform.localScale = Vector3.one * 0.9f;
            panel.transform.position = new Vector3(panel.transform.position.x, panel.transform.position.y, 0);
            _usingPanelList.Add(panel.gameObject);

            if (isCoolTime == false)
            {
                panel.CoolTimeOff();
            }
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
        if (isActive)
        {
            Define.MapScene?.mapDial.DialLock();
            Define.DialScene?.Dial.DialLock();
        }
        else
        {
            Define.MapScene?.mapDial.DialUnlock();
            Define.DialScene?.Dial.DialUnlock();
        }
        _backgroundPanel.SetActive(isActive);
        _scrollView.SetActive(isActive);
    }

    private void UpdateCoolTime()
    {
        ReturnPanels();
        SettingPanels(Managers.Deck.Deck.FindAll(x => x.IsCoolTime == true), null, true);
        EventManager.StopListening(Define.ON_START_PLAYER_TURN, UpdateCoolTime);
    }

    public bool GetUIActive()
    {
        return _backgroundPanel.activeSelf;
    }
}