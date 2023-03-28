using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoSingleton<DeckManager>
{
    [SerializeField]
    private List<RuneSO> _defaultRune = new List<RuneSO>(); // 초기 기본 지급 룬 
    [SerializeField]
    private List<Rune> _selectedDeck = new List<Rune>(); // 사전에 설정해둔 다이얼 안쪽의 1번째 줄 덱.
    [SerializeField]
    private List<Rune> _deck = new List<Rune>(); // 소지하고 있는 모든 룬 

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