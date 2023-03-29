using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}