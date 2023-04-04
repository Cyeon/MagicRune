using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Floor
{
    public int chapter;
    [Range(1, 10)]
    public int stage;
}

[System.Serializable]
public class AttackMapInfo
{
    [SerializeField] private Floor _minFloor;
    public int MinFloor => (_minFloor.chapter - 1) * 10 + _minFloor.stage;

    [SerializeField] private Floor _maxFloor;
    public int MaxFloor => (_maxFloor.chapter - 1) * 10 + _maxFloor.stage;

    public List<Enemy> enemyList = new List<Enemy>();

    public void Reset()
    {
        foreach(var enemy in enemyList)
        {
            enemy.isEnter = false;
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