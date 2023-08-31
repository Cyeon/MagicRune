using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broadening : Passive
{
    private int _increaseDamagePercent = 50;
    private int _increaseDamage;
    public int AddDmg => _increaseDamage;

    public override void Disable()
    {
        Enemy.OnTakeDamage.RemoveListener(IncreaseDamage);
        Player.OnGetDamage -= ApplyDamage;
    }

    public override void Init()
    {
        Enemy.OnTakeDamage.AddListener(IncreaseDamage);
        Player.OnGetDamage += ApplyDamage;
    }

    private void IncreaseDamage(float damage)
    {
        _increaseDamage += Mathf.FloorToInt(damage * (_increaseDamagePercent / 100));
    }
    
    private void ApplyDamage()
    {
        Player.currentDmg += _increaseDamage;
    }
}
