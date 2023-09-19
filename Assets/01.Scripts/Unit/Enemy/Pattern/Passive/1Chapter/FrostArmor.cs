using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostArmor : Passive
{
    public override void Disable()
    {
        Enemy.OnTakeDamage.RemoveListener(DamageFrost);
    }

    public override void Init()
    {
        Enemy.OnTakeDamage.AddListener(DamageFrost);
    }

    private void DamageFrost(float damage)
    {
        if(damage > 0)
        {
            Player.StatusManager.AddStatus(StatusName.Chilliness, 1);
        }
    }
}
