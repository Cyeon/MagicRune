using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public Player player;

    private int _gold = 0;
    public int Gold { get=>_gold; set => _gold = value; }
}
