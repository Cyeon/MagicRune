using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackMapInfo
{
    [Range(0, 9)] public int minStage;
    [Range(0, 9)] public int maxStage;

    public List<Enemy> enemyList = new List<Enemy>();

    public void Reset()
    {
        foreach(var enemy in enemyList)
        {
            enemy.isEnter = false;
        }
    }
}