using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackMapInfo
{
    public int minFloor = 1;
    public int maxFloor = 1;
    public List<EnemySO> enemyList = new List<EnemySO>();
}

[CreateAssetMenu(menuName = "Map/Attack/Attack Map List")]
public class AttackMapListSO : ScriptableObject
{
    public List<AttackMapInfo> map;
}
