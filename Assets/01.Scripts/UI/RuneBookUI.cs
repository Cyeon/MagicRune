using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 도감 UI에 사용되는 스크립트 
/// </summary>
public class RuneBookUI : RuneListViewUI
{
    #region System
    private List<BaseRuneSO> _orederList = new List<BaseRuneSO>();

    private bool _rarityAscending = true;
    private bool _nameAscending = true;
    #endregion

    private RuneBookPanel _template = null;

    [SerializeField]
    private Transform _orderByButtons = null;

    protected override void Start()
    {
        _backgroundPanel = transform.Find("RuneListView_BGPanel").gameObject;
        _scrollView = transform.Find("RuneListView_ScrollView").gameObject;
        _content = _scrollView.transform.Find("Viewport").GetChild(0).transform;

        ActiveUI(true);
        _template = Managers.Resource.Load<RuneBookPanel>("Prefabs/UI/RunePanel/RuneBook");

        _orderByButtons.Find("RarityButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            _rarityAscending = !_rarityAscending;
            ArrowFlip(_rarityAscending, _orderByButtons.Find("RarityButton").Find("Image"));
            ChangeOrderBy();
        });

        _orderByButtons.Find("NameButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            _nameAscending = !_nameAscending;
            ArrowFlip(_nameAscending, _orderByButtons.Find("NameButton").Find("Image"));
            ChangeOrderBy();
        });
    }

    private void Init()
    {

    }

    public void ChangeIndex(int typeIndex)
    {
        AttributeType type = (AttributeType)typeIndex;
        _orederList.Clear();
        _orederList = Managers.Deck.RuneDictionary[type].ToList();

        ChangeOrderBy();
    }

    public void ChangeOrderBy()
    {
        if (_rarityAscending)
        {
            if (_nameAscending)
                _orederList = _orederList.OrderBy(x => x.Rarity).ThenBy(x => x.RuneName).ToList();
            else
                _orederList = _orederList.OrderBy(x => x.Rarity).ThenByDescending(x => x.RuneName).ToList();
        }
        else
        {
            if (_nameAscending)
                _orederList = _orederList.OrderByDescending(x => x.Rarity).ThenBy(x => x.RuneName).ToList();
            else
                _orederList = _orederList.OrderByDescending(x => x.Rarity).ThenByDescending(x => x.RuneName).ToList();
        }

        SettingPanels();
    }

    private void SettingPanels()
    {
        ReturnPanels();

        for (int i = 0; i < _orederList.Count; i++)
        {
            RuneBookPanel panel = Managers.Resource.Instantiate(_template.gameObject, _content).GetComponent<RuneBookPanel>();
            panel.SetUI(_orederList[i], false);
            panel.transform.localScale = Vector3.one * 0.9f;
            panel.transform.position = new Vector3(panel.transform.position.x, panel.transform.position.y, 0);

            _usingPanelList.Add(panel.gameObject);
        }
    }

    public void SetActiveUIFirst()
    {
        ChangeIndex(0);

        ActiveUI(true);
    }

    private void ArrowFlip(bool isAscending, Transform arrow)
    {
        Vector3 rotation = arrow.eulerAngles;

        if (isAscending)
            rotation.z = 90f;
        else
            rotation.z = 270f;

        arrow.eulerAngles = rotation;
    }
}