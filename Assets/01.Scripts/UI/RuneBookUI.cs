using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 도감 UI에 사용되는 스크립트 
/// </summary>
public class RuneBookUI : MonoBehaviour
{
    [SerializeField]
    private RuneBookPanel _template = null;

    private List<BaseRuneSO> _orederList = new List<BaseRuneSO>();

    private bool _rarityAscending = true;
    private bool nameAscending = true;

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
            if (nameAscending)
                _orederList.OrderBy(x => x.Rarity).ThenBy(x => x.RuneName);
            else
                _orederList.OrderBy(x => x.Rarity).ThenByDescending(x => x.RuneName);
        }
        else
        {
            if (nameAscending)
                _orederList.OrderByDescending(x => x.Rarity).ThenBy(x => x.RuneName);
            else
                _orederList.OrderByDescending(x => x.Rarity).ThenByDescending(x => x.RuneName);
        }
    }

    private void ReturnPanels()
    {

    }

    private void DestroyPanels()
    {

    }
}