using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FierceCharge : Passive
{
    [SerializeField] private int _increasePercent = 50;

    public override void Disable()
    {
        Player.OnGetDamage -= DefenceCheck;
    }

    public override void Init()
    {
        Player.OnGetDamage += DefenceCheck;
    }

    private void DefenceCheck()
    {
        if(Player.Shield > 0)
        {
            Player.currentDmg += Player.currentDmg * (_increasePercent / 100);
        }
    }
}
