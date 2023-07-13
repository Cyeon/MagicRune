using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 도감 UI에 사용되는 스크립트 
/// </summary>
public class RuneBookUI : MonoBehaviour
{
    [SerializeField]
    private RuneBookPanel _template = null;

    private List<BaseRuneSO> _orederList = new List<BaseRuneSO>(); 

    private void Init()
    {

    }

    public void ChangeIndex(AttributeType type, bool rarityAscending = true, bool nameAscending = true)
    {

    }

    public List<BaseRuneSO> ReturnRunes(AttributeType type)
    {
        return Managers.Deck.RuneDictionary[type];
    }

    private void ReturnPanels()
    {

    }

    private void DestroyPanels()
    {

    }
}