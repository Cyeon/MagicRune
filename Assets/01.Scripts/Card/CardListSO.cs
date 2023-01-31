using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/CardList")]
public class CardListSO : ScriptableObject
{
    public List<CardSO> cards = new List<CardSO>();
}