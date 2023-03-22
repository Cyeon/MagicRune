using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoSingleton<DeckManager>
{
    [SerializeField]
    private List<Rune> _defaultRune = new List<Rune>();

    [SerializeField]
    private List<Rune> _deck = new List<Rune>(); // �����ϰ� �ִ� ��� �� 
    [SerializeField]
    private List<Rune> _selectedDeck = new List<Rune>(); // ������ �����ص� ���̾� ������ 1��° �� ��.

    public List<Rune> Deck => _deck;
    public List<Rune> SelectedDeck => _selectedDeck;

    private void Start()
    {
        if (_deck == null)
        {
            _deck = _defaultRune;
        }   
    }

    public void AddRune(Rune rune)
    {
        _deck.Add(rune);
    }

    public void RemoveRune(Rune rune)
    {
        _deck.Remove(rune);
    }
}