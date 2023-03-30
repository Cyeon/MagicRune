using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DeckManager : MonoSingleton<DeckManager>
{
    public List<RuneSO> _defaultRune = new List<RuneSO>(); // �ʱ� �⺻ ���� �� 

    public const int FIRST_DIAL_DECK_MAX_COUNT = 6; // ù��° ���̾� �� �ִ� ����

    [SerializeField]
    private List<Rune> _firstDialDeck = new List<Rune>(); // ������ �����ص� ���̾� ������ 1��° �� ��.
    public List<Rune> FirstDialDeck => _firstDialDeck;

    [SerializeField]
    private List<Rune> _deck = new List<Rune>(); // �����ϰ� �ִ� ��� �� 
    public List<Rune> Deck => _deck;

    private void Awake()
    {
        if (_deck.Count == 0) // ���� ������� ��� �����ص� �ʱ� ���� �־��� 
        {
            for (int i = 0; i < _defaultRune.Count; i++)
            {
                Rune rune = new Rune(_defaultRune[i]);
                AddRune(rune);
            }
        }
    }

    /// <summary> Deck�� �� �߰� </summary>
    public void AddRune(Rune rune)
    {
        _deck.Add(rune);
    }

    /// <summary> Deck���� �� ����� </summary>
    public void RemoveRune(Rune rune)
    {
        _deck.Remove(rune);
    }
 
    /// <summary> FirstDialDeck�� �� �߰� </summary>
    public void SetFirstDeck(Rune rune)
    {
        _firstDialDeck.Add(rune);
    }

    /// <summary> FistDialDeck���� �� ����� </summary>
    public void RemoveFirstDeck(Rune rune)
    {
        _firstDialDeck.Remove(rune);
    }

    public void RuneSpawn(int fIndex, int sIndex)
    {
        Rune tempRune = _deck[fIndex];
        _deck[fIndex] = _deck[sIndex];
        _deck[sIndex] = tempRune;
    }

    public void UsingDeckSort()
    {
        List<Rune> usingRune = new List<Rune>();
        List<Rune> notUsingRune = new List<Rune>();

        usingRune = _deck.Where(x => x.IsCoolTime == false).ToList();
        notUsingRune = _deck.Where(x => x.IsCoolTime == true).ToList();

        _deck.Clear();
        for(int i = 0; i < usingRune.Count; i++)
        {
            _deck.Add(usingRune[i]);
        }
        for (int i = 0; i < notUsingRune.Count; i++)
        {
            _deck.Add(notUsingRune[i]);
        }
    }

    public int GetUsingRuneCount()
    {
        int count = 0;
        for(int i = 0; i < _deck.Count; i++)
        {
            if (_deck[i].IsCoolTime == false)
            {
                count++;
            }
        }

        return count;
    }
}