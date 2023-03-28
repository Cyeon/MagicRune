using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoSingleton<DeckManager>
{
    [SerializeField]
    private List<RuneSO> _defaultRune = new List<RuneSO>(); // �ʱ� �⺻ ���� �� 
    [SerializeField]
    private List<Rune> _selectedDeck = new List<Rune>(); // ������ �����ص� ���̾� ������ 1��° �� ��.
    [SerializeField]
    private List<Rune> _deck = new List<Rune>(); // �����ϰ� �ִ� ��� �� 

    public List<Rune> Deck => _deck;
    public List<Rune> SelectedDeck => _selectedDeck;

    private void Awake()
    {
        if (_deck.Count==0)
        {
            for (int i = 0; i < _defaultRune.Count; i++)
            {
                Rune rune = new Rune(_defaultRune[i]);
                //rune.SetMagic(_defaultRune[i]);
                AddRune(rune);
            }
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