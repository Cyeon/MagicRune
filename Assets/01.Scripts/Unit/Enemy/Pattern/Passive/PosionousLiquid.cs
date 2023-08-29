using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosionousLiquid : Passive
{
    [SerializeField] private int _value = 5;

    public override void Disable()
    {
        Managers.GetPlayer().OnTakeDamage.RemoveListener(Poison);
    }

    public override void Init()
    {
        Managers.GetPlayer().OnTakeDamage.AddListener(Poison);
    }

    private void Poison(float dmg)
    {
        if(dmg > 0)
        {
            Managers.GetPlayer().StatusManager.AddStatus(StatusName.PoisonousLiquid, _value);
        }
    }
}
