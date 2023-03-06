using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapManager : MonoSingleton<MapManager>
{
    [SerializeField]
    private AttackMapListSO _attackMap;
    private List<AttackMapInfo> _currentMapList;

    private void Start()
    {
        DontDestroyOnLoad(this);
        _currentMapList = _attackMap.map.ToList();
        _currentMapList[0].maxFloor = 10;
    }
}
