using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MyBox;

public class DeckManager
{
    private List<BaseRune> _defaultRune = new List<BaseRune>(20); // �ʱ� �⺻ ���� ��
    public List<BaseRune> DefaultRune => _defaultRune;

    public const int FIRST_DIAL_DECK_MAX_COUNT = 3; // ù��° ���̾� �� �ִ� ����

    private List<BaseRune> _firstDialDeck = new List<BaseRune>(3); // ������ �����ص� ���̾� ������ 1��° �� ��.
    public List<BaseRune> FirstDialDeck => _firstDialDeck;

    private List<BaseRune> _deck = new List<BaseRune>(12); // �����ϰ� �ִ� ��� ��
    public List<BaseRune> Deck => _deck;

    public void Init()
    {
        // ���߿� Json �����ϸ� ���⼭ �ҷ����� ���� �ϰ���...
    }

    public void SetDefaultDeck(List<BaseRune> runeList)
    {
        for(int i = 0; i < runeList.Count; i++)
        {
            AddRune(runeList[i]);
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

    /// <summary> Deck�� �� �߰� </summary>
    public void AddRune(BaseRune rune)
    {
        _deck.Add(rune);
    }

    /// <summary> Deck���� �� ����� </summary>
    public void RemoveDeck(BaseRune rune)
    {
        _deck.Remove(rune);
    }
 
    /// <summary> FirstDialDeck�� �� �߰� </summary>
    public void AddRuneFirstDeck(BaseRune rune)
    {
        _firstDialDeck.Add(rune);
    }

    /// <summary> FistDialDeck���� �� ����� </summary>
    public void RemoveFirstDeck(BaseRune rune)
    {
        _firstDialDeck.Remove(rune);
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
        //List<BaseRune> usingRune = new List<BaseRune>();
        //List<BaseRune> notUsingRune = new List<BaseRune>();

        //usingRune = _deck.Where(x => x.IsCoolTime == false).ToList();
        //notUsingRune = _deck.Where(x => x.IsCoolTime == true).ToList();


        //_deck.Clear();
        //for(int i = 0; i < usingRune.Count; i++)
        //{
        //    _deck.Add(usingRune[i]);
        //}
        //for (int i = 0; i < notUsingRune.Count; i++)
        //{
        //    _deck.Add(notUsingRune[i]);
        //}

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