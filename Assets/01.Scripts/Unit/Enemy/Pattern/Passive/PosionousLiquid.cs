using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosionousLiquid : Passive
{
    [SerializeField] private int _value = 5;

    public override void Disable()
    {
        Player.OnTakeDamage.RemoveListener(Poison);
    }

    public override void Init()
    {
        Player.OnTakeDamage.AddListener(Poison);
    }

    private void Poison(float dmg)
    {
        if(dmg > 0)
        {
            Player.StatusManager.AddStatus(StatusName.PoisonousLiquid, _value);
        }
    }
}
