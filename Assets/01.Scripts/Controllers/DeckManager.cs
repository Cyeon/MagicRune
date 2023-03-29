using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoSingleton<DeckManager>
{
    public List<RuneSO> _defaultRune = new List<RuneSO>(); // 초기 기본 지급 룬 

    public const int FIRST_DIAL_DECK_MAX_COUNT = 6; // 첫번째 다이얼 덱 최대 개수

    [SerializeField]
    private List<Rune> _firstDialDeck = new List<Rune>(); // 사전에 설정해둔 다이얼 안쪽의 1번째 줄 덱.
    public List<Rune> FirstDialDeck => _firstDialDeck;

    [SerializeField]
    private List<Rune> _deck = new List<Rune>(); // 소지하고 있는 모든 룬 
    public List<Rune> Deck => _deck;

    private void Awake()
    {
        if (_deck.Count == 0) // 덱이 비어있을 경우 설정해둔 초기 덱을 넣어줌 
        {
            for (int i = 0; i < _defaultRune.Count; i++)
            {
                Rune rune = new Rune(_defaultRune[i]);
                AddRune(rune);
            }
        }
    }

    /// <summary> Deck에 룬 추가 </summary>
    public void AddRune(Rune rune)
    {
        _deck.Add(rune);
    }

    /// <summary> Deck에서 룬 지우기 </summary>
    public void RemoveRune(Rune rune)
    {
        _deck.Remove(rune);
    }
 
    /// <summary> FirstDialDeck에 룬 추가 </summary>
    public void SetFirstDeck(Rune rune)
    {
        _firstDialDeck.Add(rune);
    }

    /// <summary> FistDialDeck에서 룬 지우기 </summary>
    public void RemoveFirstDeck(Rune rune)
    {
        _firstDialDeck.Remove(rune);
    }
}