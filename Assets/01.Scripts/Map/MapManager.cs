using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public  enum MapType
{
    Attack,
    Rest
}

[System.Serializable]
public class MapInfo
{
    public MapType type;
    public float percent;
}

public class MapManager : MonoSingleton<MapManager>
{
    public static EnemySO SelectEnemy;
    public AttackMapListSO attackMap;

    public List<MapInfo> mapList = new List<MapInfo>();

    private int _floor = 1;
    public int Floor => _floor;

    [SerializeField]
    private List<MapPanel> _mapPanelList = new List<MapPanel>();

    private void Start()
    {
        MapInit();
    }

    private void MapInit()
    {
        foreach(MapPanel panel in _mapPanelList)
        {
            switch(GetMapType())
            {
                case MapType.Attack:
                    panel.Init(GetAttackEnemy());
                    break;

                case MapType.Rest:
                    panel.Init(MapType.Rest);
                    break;

                default:
                    break;
            }
        }
    }

    private MapType GetMapType()
    {
        float sum = 0f;
        foreach(var map in mapList)
        {
            sum += map.percent;
        }

        float value = Random.Range(0, sum);
        float temp = 0f;

        for(int i = 0; i < mapList.Count; i++) 
        { 
            if(value >= temp && value < temp + mapList[i].percent)
                return mapList[i].type;

            temp += mapList[i].percent;
        }

        return MapType.Attack;
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
        if(enemyList.Count == 0)
        {
            return attackMap.map[0].enemyList[0];
        }
        enemyList[idx].IsEnter = true;
        return enemyList[idx];
    }
}
