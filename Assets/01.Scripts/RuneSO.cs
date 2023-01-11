using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Rune")]
public class RuneSO : ScriptableObject
{
    public string cardName;
    public int cost;
    public string cardDescription;

    [Min(0)]
    public int mainRuneValue;
    [Min(0)]
    public int assistRuneValue;
}
