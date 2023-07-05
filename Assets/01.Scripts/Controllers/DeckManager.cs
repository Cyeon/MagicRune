using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MyBox;

public class DeckManager
{
    private List<BaseRuneSO> _defaultRune = new List<BaseRuneSO>(20); // 珥덇린 湲곕낯 吏湲?猷?
    public List<BaseRuneSO> DefaultRune => _defaultRune;

    private List<BaseRune> _deck = new List<BaseRune>(12); // ?뚯??섍퀬 ?덈뒗 紐⑤뱺 猷?
    public List<BaseRune> Deck => _deck;

    public void Init()
    {
        SetDefaultDeck(Managers.Resource.Load<DeckSO>("SO/Deck/DefaultDeck").RuneList);
    }

    public void SetDefaultDeck(in List<BaseRuneSO> runeList)
    {
        _deck.Clear();

        for (int i = 0; i < runeList.Count; i++)
        {
            Debug.Log(Managers.Rune.GetRune(runeList[i]));
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
        return newRuneList[idx];
    }

    public void DeckSort()
    {
        _deck = _deck.OrderByDescending(x => x.BaseRuneSO.AttributeType).ThenBy(x => x.BaseRuneSO.CoolTime).ToList();
    }
}