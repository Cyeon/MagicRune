using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Unit/Enemy")]
public class EnemySO : ScriptableObject
{
    public string enemyName;

    public float health;

    public GameObject prefab;

    public List<PatternEnum> patternList = new List<PatternEnum>();
}
