using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapManager : MonoSingleton<MapManager>
{
    public static EnemySO SelectEnemy;
    public AttackMapListSO attackMap;

    private int _floor = 1;
    public int Floor => _floor;

    [SerializeField]
    private List<MapPanel> _mapPanelList = new List<MapPanel>();

    public void MapInit()
    {
        foreach(MapPanel panel in _mapPanelList)
        {
            panel.Init(GetAttackEnemy());
        }
    }

    private EnemySO GetAttackEnemy()
    {
        List<EnemySO> enemyList = new List<EnemySO> ();
        for(int i = 0; i < attackMap.map.Count; ++i)
        {
            if (attackMap.map[i].minFloor <=  Floor && attackMap.map[i].maxFloor >= Floor)
            {
                foreach(var enemy in attackMap.map[i].enemyList)
                {
                    if(!enemy.IsEnter) enemyList.Add (enemy);
                }
            }
        }
            
        int idx = Random.Range(0, enemyList.Count);
        Debug.Log(idx);
        enemyList[idx].IsEnter = true;
        return enemyList[idx];
    }
}
