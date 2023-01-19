using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/CardList")]
public class CardListSO : ScriptableObject
{
    public List<GameObject> cards = new List<GameObject>();
}