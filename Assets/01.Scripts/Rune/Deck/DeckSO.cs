using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Deck/Deck")]
public class DeckSO : ScriptableObject
{
    public List<BaseRuneSO> RuneList;
}
