using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public Player player;

    private void Start()
    {
        MapManager.Instance.attackMap.Reset();
        MapManager.Instance.MapInit();
    }
}
