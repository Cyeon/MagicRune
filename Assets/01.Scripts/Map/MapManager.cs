using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapManager : MonoSingleton<MapManager>
{
    [SerializeField]
    private AttackMapListSO _attackMap;
    public static EnemySO selectEnemy;

    private int _floor = 1;
    public int Floor => _floor;

    [SerializeField]
    private List<MapPanel> _mapPanelList = new List<MapPanel>();

    private void Awake()
    {

    }

    private void Start()
    {
        _attackMap.Reset();
        MapInit();
    }

    private void MapInit()
    {
        foreach(MapPanel panel in _mapPanelList)
        {
            panel.Init(GetAttackEnemy());
        }
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
