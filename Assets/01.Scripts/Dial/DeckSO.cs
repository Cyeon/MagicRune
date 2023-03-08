using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CardList
{
    public List<CardSO> list;
}

[CreateAssetMenu(menuName = "SO/Dial/Deck")]
public class DeckSO : ScriptableObject
{
    public List<CardList> list;
}
