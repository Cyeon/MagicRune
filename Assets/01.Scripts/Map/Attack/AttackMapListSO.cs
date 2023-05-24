using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackMapInfo
{
    public PeriodType periodType;
    public List<Enemy> enemyList = new List<Enemy>();

    public void Reset()
    {
        foreach(var enemy in enemyList)
        {
            enemy.isEnter = false;
        }
    }
}