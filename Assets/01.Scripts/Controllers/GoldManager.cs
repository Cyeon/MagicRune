using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldManager
{
    private int _gold = 100;
    public int Gold { get => _gold; private set => _gold = value; }

    public void Init()
    {
        _gold = int.MaxValue;
    }

    public void AddGold(int amount)
    {
        _gold += amount;
    }
}
