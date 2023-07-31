using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MyBox;
using UnityEditor;

public class DeckManager
{
    private List<BaseRuneSO> _defaultRune = new List<BaseRuneSO>(20); // 珥덇린 湲곕낯 吏湲?猷?
    public List<BaseRuneSO> DefaultRune => _defaultRune;

    private List<BaseRune> _deck = new List<BaseRune>(12); // ?뚯??섍퀬 ?덈뒗 紐⑤뱺 猷?
    public List<BaseRune> Deck => _deck;

    private DeckSO _allRuneSO = null;
    private Dictionary<AttributeType, List<BaseRuneSO>> _runeDictionary = new Dictionary<AttributeType, List<BaseRuneSO>>();
    public Dictionary<AttributeType, List<BaseRuneSO>> RuneDictionary => _runeDictionary;

    public void Init()
    {
        SetDefaultDeck(Managers.Resource.Load<DeckSO>("SO/Deck/DefaultDeck").RuneList);

        _allRuneSO = Managers.Resource.Load<DeckSO>("SO/Deck/AllRuneDeck");

        for (int i = 0; i < (int)AttributeType.MAX_COUNT; i++)
        {
            if (_runeDictionary.ContainsKey((AttributeType)i)) // 혹시라도 먼저 만들어져있다면 패스 
                continue;
            _runeDictionary.Add((AttributeType)i, new List<BaseRuneSO>()); // 미리 속성 별 리스트 만들어서 할당 시켜놓기 
        }

        for (int i = 0; i < _allRuneSO.RuneList.Count; i++)
        {
            if (!_runeDictionary[AttributeType.None].Contains(_allRuneSO.RuneList[i]))
                _runeDictionary[AttributeType.None].Add(_allRuneSO.RuneList[i]);    // 전체 리스트에 추가
            if (!_runeDictionary[_allRuneSO.RuneList[i].AttributeType].Contains(_allRuneSO.RuneList[i]))
                _runeDictionary[_allRuneSO.RuneList[i].AttributeType].Add(_allRuneSO.RuneList[i]); // 각 속성 리스트에 추가
        }
    }

    public void SetDefaultDeck(in List<BaseRuneSO> runeList)
    {
        _deck.Clear();

        for (int i = 0; i < runeList.Count; i++)
        {
            if (_defaultRune.Contains(runeList[i]) == false)
            {
                _defaultRune.Add(runeList[i]);
            }
            AddRune(Managers.Rune.GetRune(runeList[i]));
        }

        RuneInit();
    }

    private void RuneInit()
    {
        for (int i = 0; i < _deck.Count; i++)
        {
            _deck[i].Init();
        }
    }

    /// <summary> Deck??猷?異붽? </summary>
    public void AddRune(BaseRune rune)
    {
        if (rune.BaseRuneSO.DiscoveryType != DiscoveryType.Known)
        {
            AssetDatabase.StartAssetEditing();
            rune.BaseRuneSO.DiscoveryType = DiscoveryType.Known;
            AssetDatabase.StopAssetEditing();
            EditorUtility.SetDirty(rune.BaseRuneSO);
        }

        _deck.Add(rune);
        DeckSort();
    }

    /// <summary> Deck?먯꽌 猷?吏?곌린 </summary>
    public void RemoveDeck(BaseRune rune)
    {
        _deck.Remove(rune);
    }

    public void RuneSwap(int fIndex, int sIndex)
    {
        if (fIndex == sIndex) return;

        BaseRune tempRune = _deck[fIndex];
        _deck[fIndex] = _deck[sIndex];
        _deck[sIndex] = tempRune;
    }

    public void UsingDeckSort()
    {
        List<BaseRune> newDeck = _deck.OrderBy(x => x.IsCoolTime == false)/*.ThenBy(x => x.IsUsing == false)*/.ToList();
        _deck.Clear();
        _deck = new List<BaseRune>(newDeck);
    }

    public int GetUsingRuneCount()
    {
        int count = 0;
        for (int i = 0; i < _deck.Count; i++)
        {
            if (_deck[i].IsCoolTime == false && _deck[i].IsUsing == false)
            {
                count++;
            }
        }

        return count;
    }

    public BaseRune GetRandomRune(List<BaseRune> ignoreRuneList = null)
    {
        List<BaseRune> newRuneList = new List<BaseRune>(Deck);

        if (ignoreRuneList != null)
        {
            for (int i = 0; i < ignoreRuneList.Count; i++)
            {
                newRuneList.Remove(ignoreRuneList[i]);
            }
        }
        int idx = Random.Range(0, newRuneList.Count);

        if (newRuneList[idx].BaseRuneSO.DiscoveryType == DiscoveryType.Unknwon)
        {
            AssetDatabase.StartAssetEditing();
            newRuneList[idx].BaseRuneSO.DiscoveryType = DiscoveryType.Find;
            AssetDatabase.StopAssetEditing();
            EditorUtility.SetDirty(newRuneList[idx].BaseRuneSO);
        }

        return newRuneList[idx];
    }

    public void DeckSort()
    {
        _deck = _deck.OrderByDescending(x => x.BaseRuneSO.AttributeType).ThenBy(x => x.BaseRuneSO.CoolTime).ToList();
    }
}