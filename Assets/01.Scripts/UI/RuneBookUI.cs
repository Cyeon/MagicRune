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
    private Transform _indexButtons = null;

    protected override void Start()
    {
        base.Start();
        _template = Managers.Resource.Load<RuneBookPanel>("Prefabs/UI/RunePanel/RuneBook");

        for (int i = 0; i < _indexButtons.childCount; i++)
        {
            _indexButtons.GetChild(i).GetComponent<Button>().onClick.AddListener(() => ChangeIndex((AttributeType)i));
        }
    }

    private void Init()
    {
        
    }

    public void ChangeIndex(AttributeType type)
    {
        _orederList.Clear();
        _orederList = Managers.Deck.RuneDictionary[type];

        ChangeOrderBy();
    }

    public void ChangeOrderBy()
    {
        if (_rarityAscending)
        {
            if (_nameAscending)
                _orederList.OrderBy(x => x.Rarity).ThenBy(x => x.RuneName);
            else
                _orederList.OrderBy(x => x.Rarity).ThenByDescending(x => x.RuneName);
        }
        else
        {
            if (_nameAscending)
                _orederList.OrderByDescending(x => x.Rarity).ThenBy(x => x.RuneName);
            else
                _orederList.OrderByDescending(x => x.Rarity).ThenByDescending(x => x.RuneName);
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
        ChangeIndex(AttributeType.None);

        ActiveUI(true);
    }
}