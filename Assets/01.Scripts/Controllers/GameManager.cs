using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public Player player;

    private int _gold = 100;
    public int Gold { get => _gold; set => _gold = value; }

    private void Awake()
    {
        Application.targetFrameRate = 30;
    }

    public void AddGold(int amount)
    {
        _gold += amount;
    }
}
