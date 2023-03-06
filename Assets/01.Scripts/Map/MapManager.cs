using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapManager : MonoSingleton<MapManager>
{
    [SerializeField]
    private AttackMapListSO _attackMap;

    private int _floor = 1;
    public int Floor => _floor;

    private void Start()
    {
        DontDestroyOnLoad(this);
        _attackMap.Reset();
        Debug.Log(GetAttackEnemy().enemyName);
        Debug.Log(GetAttackEnemy().enemyName);
    }

    private EnemySO GetAttackEnemy()
    {
        List<EnemySO> enemyList = new List<EnemySO> ();
        for(int i = 0; i < _attackMap.map.Count; ++i)
        {
            if (_attackMap.map[i].minFloor <=  Floor && _attackMap.map[i].maxFloor >= Floor)
            {
                foreach(var enemy in _attackMap.map[i].enemyList)
                {
                    if(!enemy.IsEnter) enemyList.Add (enemy);
                }
            }
        }

        int idx = Random.Range(0, enemyList.Count);
        enemyList[idx].IsEnter = true;
        return enemyList[idx];
    }
}
