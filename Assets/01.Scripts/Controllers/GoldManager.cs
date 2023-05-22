using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldManager
{
    private int _gold = 100;
    public int Gold { get => _gold; private set => _gold = value; }

    public Action UpdateGoldAction;

    public void Init()
    {
        ResetGoldAmount();
        
        UpdateGoldAction?.Invoke();
    }

    public void AddGold(int amount)
    {
        _gold = Mathf.Clamp(_gold + amount, 0, int.MaxValue);
        UpdateGoldAction?.Invoke();
    }

    public void ResetGoldAmount()
    {
        _gold = 100;
    }
}