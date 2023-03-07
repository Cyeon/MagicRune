using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Unit/Enemy")]
public class EnemySO : ScriptableObject
{
    public string enemyName;
    public float health;
    [ShowAssetPreview(32, 32), Tooltip("æ∆¿Ãƒ‹")] 
    public Sprite icon;

    public GameObject prefab;
    public bool IsEnter = false;

    public List<Pattern> patternList;
}
