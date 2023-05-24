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
        Define.DialScene?.GoldPopUp(amount);
        Define.MapScene?.GoldPopUp(amount);

        UpdateGoldAction?.Invoke();
        Managers.Sound.StopSound(SoundType.Effect);
        Managers.Sound.PlaySound("SFX/GoldGetSound", SoundType.Effect);
    }

    public void ResetGoldAmount()
    {
        _gold = 100;
    }
}