using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MyBox;

public class DeckManager
{
    private List<BaseRune> _defaultRune = new List<BaseRune>(20); // 초기 기본 지급 룬
    public List<BaseRune> DefaultRune => _defaultRune;

    public const int FIRST_DIAL_DECK_MAX_COUNT = 3; // 첫번째 다이얼 덱 최대 개수

    //private List<BaseRune> _firstDialDeck = new List<BaseRune>(3); // 사전에 설정해둔 다이얼 안쪽의 1번째 줄 덱.
    //public List<BaseRune> FirstDialDeck => _firstDialDeck;

    private List<BaseRune> _deck = new List<BaseRune>(12); // 소지하고 있는 모든 룬
    public List<BaseRune> Deck => _deck;

    public void Init()
    {
        // 나중에 Json 저장하면 여기서 불러오기 등을 하겠지...
    }

    public void SetDefaultDeck(List<BaseRune> runeList)
    {
        _deck.Clear();

        for(int i = 0; i < runeList.Count; i++)
        {
            AddRune(runeList[i]);
        }

        RuneInit();
    }

    public void SetDefaultDeck(List<BaseRuneSO> runeList)
    {
        _deck.Clear();

        for (int i = 0; i < runeList.Count; i++)
        {
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

    /// <summary> Deck에 룬 추가 </summary>
    public void AddRune(BaseRune rune)
    {
        _deck.Add(rune);
    }

    /// <summary> Deck에서 룬 지우기 </summary>
    public void RemoveDeck(BaseRune rune)
    {
        _deck.Remove(rune);
    }

    ///// <summary> FirstDialDeck에 룬 추가 </summary>
    //public void AddRuneFirstDeck(BaseRune rune)
    //{
    //    _firstDialDeck.Add(rune);
    //}

    ///// <summary> FistDialDeck에서 룬 지우기 </summary>
    //public void RemoveFirstDeck(BaseRune rune)
    //{
    //    _firstDialDeck.Remove(rune);
    //}

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
        for(int i = 0; i < _deck.Count; i++)
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
}