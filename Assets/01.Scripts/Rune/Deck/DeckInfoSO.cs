using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Deck/DeckInfo")]
public class DeckInfoSO : ScriptableObject
{
    public Sprite DeckImage;
    public string DeckName;
    public string DeckDescription;
    public DeckSO DeckSO;
}
