using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackMapInfo
{
    public int minFloor = 1;
    public int maxFloor = 1;
    public List<EnemySO> enemyList = new List<EnemySO>();

    public void Reset()
    {
        foreach(var enemy in enemyList)
        {
            enemy.IsEnter = false;
        }
    }
}

[CreateAssetMenu(menuName = "Map/Attack/Attack Map List")]
public class AttackMapListSO : ScriptableObject
{
    public List<AttackMapInfo> map;

    public void EnterReset()
    {
        foreach(var m in map)
        {
            m.Reset();
        }
        Debug.Log("¸®¼Â");
    }
}