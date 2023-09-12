using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuneListViewUI : MonoBehaviour
{
    protected GameObject _backgroundPanel = null;
    protected GameObject _scrollView = null;
    protected CanvasGroup _canvasGroup = null;

    protected Transform _content = null;
    protected List<GameObject> _usingPanelList = new List<GameObject>();

    private bool _isOpenUI = false;

    protected virtual void Start()
    {
        _backgroundPanel = transform.Find("RuneListView_BGPanel").gameObject;
        _scrollView = transform.Find("RuneListView_ScrollView").gameObject;
        _canvasGroup = _scrollView.GetComponent<CanvasGroup>();
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
        ActiveUI();
    }
    /// <summary>
    /// 사용할 수 있는 룬
    /// </summary>
    public void SetUseRunes()
    {
        SettingPanels(Managers.Deck.Deck.FindAll(x => x.IsCoolTime == false), Define.DialScene?.Dial.ConsumeDeck);
        ActiveUI();
    }

    public void SetAllRunes()
    {
        SettingPanels(Managers.Deck.Deck);
        ActiveUI();
    }

    private void SettingPanels(List<BaseRune> baseRuneList, List<BaseRune> ignoreRuneList = null, bool isCoolTime = false)
    {
        ReturnPanels();

        for (int i = 0; i < baseRuneList.Count; i++)
        {
            if (ignoreRuneList != null)
                if (ignoreRuneList.Contains(baseRuneList[i])) { continue; }
            CoolTimeRunePanel panel = Managers.Resource.Instantiate("UI/RunePanel/CoolTime", _content).GetComponent<CoolTimeRunePanel>();
            panel.SetRune(baseRuneList[i]);
            panel.SetUI(baseRuneList[i].BaseRuneSO, baseRuneList[i].IsEnhanced);
            panel.transform.localScale = Vector3.one * 0.9f;
            panel.transform.GetComponent<RectTransform>().DOAnchorPos3DZ(0, 0);
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

    protected void ActiveUI(bool isActive)
    {
        _isOpenUI = isActive;

        if (isActive)
        {
            _backgroundPanel.SetActive(isActive);
            _scrollView.SetActive(isActive);
            Define.MapScene?.mapDial.DialLock();
            Define.DialScene?.Dial.DialLock();

            _scrollView.transform.localScale = Vector3.one * 0.8f;
            _canvasGroup.DOFade(1, 0);

            Sequence seq = DOTween.Sequence();
            seq.Append(_scrollView.transform.DOScale(Vector3.one * 1.05f, 0.1f).SetEase(Ease.OutExpo));
            seq.Append(_scrollView.transform.DOScale(Vector3.one, 0.1f));
        }
        else
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(_scrollView.transform.DOScale(Vector3.one * 1.05f, 0.1f).SetEase(Ease.OutExpo));
            seq.Append(_scrollView.transform.DOScale(Vector3.one * 0.8f, 0.1f));
            seq.Join(_canvasGroup.DOFade(0, 0.1f).SetEase(Ease.InExpo));
            seq.AppendCallback(() =>
            {
                Define.MapScene?.mapDial.DialUnlock();
                Define.DialScene?.Dial.DialUnlock();
                _backgroundPanel.SetActive(isActive);
                _scrollView.SetActive(isActive);
            });
        }
    }

    protected void ActiveUI()
    {
        _isOpenUI = !_isOpenUI;
        ActiveUI(_isOpenUI);
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