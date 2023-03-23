using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Unit/Enemy")]
public class EnemySO : ScriptableObject
{
    [Header("기본정보")]
    public string enemyName;
    public float health;

    [Header("리소스")]
     [ShowAssetPreview(32, 32), Tooltip("아이콘")] 
    public Sprite icon;
    public GameObject prefab;

    [Header("패턴")]
    public List<Pattern> patternList;

    [HideInInspector] public bool IsEnter = false;
}
