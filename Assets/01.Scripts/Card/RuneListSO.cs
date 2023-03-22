using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/CardList")]
public class RuneListSO : ScriptableObject
{
    public List<RuneSO> cards = new List<RuneSO>();
}