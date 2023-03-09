using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CardList
{
    public int MaxCost;
    public List<CardSO> List;
}

[CreateAssetMenu(menuName = "SO/Dial/Deck")]
public class DeckSO : ScriptableObject
{
    public List<CardList> List;
}
