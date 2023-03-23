using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Unit/Enemy")]
public class EnemySO : ScriptableObject
{
    [Header("�⺻����")]
    public string enemyName;
    public float health;

    [Header("���ҽ�")]
     [ShowAssetPreview(32, 32), Tooltip("������")] 
    public Sprite icon;
    public GameObject prefab;

    [Header("����")]
    public List<Pattern> patternList;

    [HideInInspector] public bool IsEnter = false;
}
